using FWDays.Participants.Database;
using FWDays.Participants.Payloads;

namespace FWDays.Participants;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class Mutations
{
    public async Task<Participant> AddParticipantAsync(
        AddParticipantPayload input,
        [Service] ParticipantsDbContext context,
        CancellationToken cancellationToken)
    {
        var participant = new Participant
        {
            FirstName = input.FirstName,
            LastName = input.LastName,
            EmailAddress = input.EmailAddress,
        };

        context.Participants.Add(participant);
        await context.SaveChangesAsync(cancellationToken);

        return participant;
    }
}