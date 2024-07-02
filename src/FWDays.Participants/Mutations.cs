using FWDays.Participants.Database;
using FWDays.Participants.Processing;

namespace FWDays.Participants;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class Mutations
{
    public async Task<ParticipantRegistrationPayload> RegisterParticipantAsync(
        ParticipantRegistrationInput input, 
        [Service] ParticipantsDbContext context, 
        CancellationToken cancellationToken)
    {
        var participant = new Participant
        {
            FirstName = input.FirstName,
            LastName = input.LastName,
            EmailAddress = input.Email,
            UserName = input.Nickname
        };

        context.Participants.Add(participant);
        
        await context.SaveChangesAsync(cancellationToken);

        return new ParticipantRegistrationPayload(participant.Id, participant.FirstName, participant.LastName);
    }
}