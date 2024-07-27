using FWDays.Speakers.Processing;

namespace FWDays.Speakers;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class Mutations
{
    public async Task<SpeakerRegistrationPayload> RegisterSpeakerAsync(
        SpeakerInput input,
        [Service] SpeakersDbContext context,
        CancellationToken cancellationToken)
    {
        var speaker = new Speaker
        {
            FirstName = input.FirstName,
            LastName = input.LastName,
            Position = input.Position,
            Topic = input.Topic,
            Bio = input.Bio,
            Company = input.Company,
            Github = input.Github,
        };

        context.Speakers.Add(speaker);
        await context.SaveChangesAsync(cancellationToken);

        return new SpeakerRegistrationPayload(speaker.Id, speaker.FirstName, speaker.LastName, speaker.Topic);
    }
}