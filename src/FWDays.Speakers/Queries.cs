using FWDays.Speakers.Loaders;

namespace FWDays.Speakers;

[ExtendObjectType(OperationTypeNames.Query)]
public class Queries
{
    [UseDbContext(typeof(SpeakersDbContext))]
    [UsePaging]
    public IQueryable<Speaker> GetSpeakers([Service] SpeakersDbContext context) 
        => context.Speakers.OrderBy(t => t.Name);
    
    public static Task<Speaker> GetSpeakerByIdAsync(int id, SpeakerByIdDataLoader speakerById, CancellationToken cancellationToken)
        => speakerById.LoadAsync(id, cancellationToken);
}