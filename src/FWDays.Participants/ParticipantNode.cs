using FWDays.Participants.Database;
using FWDays.Participants.Loaders;

namespace FWDays.Participants;

[Node]
[ExtendObjectType(typeof(Participant))]
public class ParticipantNode
{
    [NodeResolver]
    public static Task<Participant> GetParticipantByIdAsync(
        int id,
        ParticipantsByIdDataLoader participantsById,
        CancellationToken cancellationToken)
        => participantsById.LoadAsync(id, cancellationToken);
}