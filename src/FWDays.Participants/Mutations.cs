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
    
    public async Task<ParticipantRegistrationPayload> ChangeRegisterEmailAsync(
        int participantId,
        string email,
        [Service] ParticipantsDbContext context, 
        CancellationToken cancellationToken)
    {
        var participant = context.Participants.FirstOrDefault(x => x.Id == participantId)
                          ?? throw new ParticipantNotFoundException(
                              $"Participant with ID {participantId} is not found");
        
        participant.EmailAddress = email;
        await context.SaveChangesAsync(cancellationToken);

        return new ParticipantRegistrationPayload(participant.Id, participant.FirstName, participant.LastName);
    }
}