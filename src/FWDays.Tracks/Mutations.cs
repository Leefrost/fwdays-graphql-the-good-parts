using FWDays.Tracks.Database;
using FWDays.Tracks.Payloads;

namespace FWDays.Tracks;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class Mutations
{
    public async Task<Track> AddTrackAsync(
        AddTrackPayload input,
        [Service] TracksDbContext context,
        CancellationToken cancellationToken)
    {
        var track = new Track
        {
            Name = input.Name,
        };

        context.Tracks.Add(track);
        await context.SaveChangesAsync(cancellationToken);

        return track;
    }
}