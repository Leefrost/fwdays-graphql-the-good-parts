using FWDays.Speakers.Processing;

namespace FWDays.Speakers;

[Node]
[ExtendObjectType(typeof(Speaker))]
public class SpeakerNode
{
    [BindMember(nameof(Speaker.Bio), Replace = true)]
    public string? GetBio([Parent] Speaker speaker, bool error = false)
    {
        if (error)
        {
            throw new GraphQLException("Some error with the bio.");
        }

        return speaker.Bio;
    }

    [NodeResolver]
    public static Task<Speaker> GetSpeakerByIdAsync(
        int id,
        SpeakerByIdDataLoader speakerById,
        CancellationToken cancellationToken)
        => speakerById.LoadAsync(id, cancellationToken);
}