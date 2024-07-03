using FWDays.Tracks.Database;
using FWDays.Tracks.Processing;

namespace FWDays.Tracks;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class Mutations
{
    public async Task<CreateTrackPayload> CreateTrackAsync(
        TrackInput input,
        [Service] TracksDbContext context,
        CancellationToken cancellationToken)
    {
        var track = new Track
        {
            Topic = input.Topic,
            Speaker = new TrackSpeaker
            {
                FirstName = input.Speaker.FirstName,
                LastName = input.Speaker.LastName
            } 
        };

        context.Tracks.Add(track);
        await context.SaveChangesAsync(cancellationToken);

        return new CreateTrackPayload(track.Id, track.Topic, $"{track.Speaker.LastName} {track.Speaker.FirstName}");
    }
    
    public async Task<AssignParticipantPayload> AssignParticipantAsync(
        int trackId,
        TrackParticipantInput participantInput,
        [Service] TracksDbContext context,
        CancellationToken cancellationToken)
    {
        var track = context.Tracks.FirstOrDefault(x => x.Id == trackId)
                    ?? throw new Exception();
        
        track.Participants.Add(new TrackParticipant
        {
            FirstName = participantInput.FirstName,
            LastName = participantInput.LastName
        });

        await context.SaveChangesAsync(cancellationToken);
        return new AssignParticipantPayload(track.Id, $"{participantInput.LastName} {participantInput.FirstName}");
    }
}