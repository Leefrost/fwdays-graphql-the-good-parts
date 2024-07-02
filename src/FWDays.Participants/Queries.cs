using FWDays.Participants.Database;
using FWDays.Participants.Processing;

namespace FWDays.Participants;

[ExtendObjectType(OperationTypeNames.Query)]
public class Queries
{
    [UseDbContext(typeof(ParticipantsDbContext))]
    [UsePaging]
    public IQueryable<Participant> GetParticipants([Service] ParticipantsDbContext context) 
        => context.Participants.OrderBy(t => t.LastName);
    
    public static Task<Participant> GetParticipantByIdAsync(int id, ParticipantsByIdDataLoader participantsById, CancellationToken cancellationToken)
        => participantsById.LoadAsync(id, cancellationToken);
}