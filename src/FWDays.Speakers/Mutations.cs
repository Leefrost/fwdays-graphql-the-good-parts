using FWDays.Speakers.Payloads;

namespace FWDays.Speakers;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class Mutations
{
    public async Task<Speaker> AddSpeakerAsync(
        AddSpeakerPayload input,
        [Service] SpeakersDbContext context,
        CancellationToken cancellationToken)
    {
        var speaker = new Speaker
        {
            Name = input.Name,
            Bio = input.Bio,
            Company = input.Company
        };

        context.Speakers.Add(speaker);
        await context.SaveChangesAsync(cancellationToken);

        return speaker;
    }
}