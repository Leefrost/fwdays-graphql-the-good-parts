using FWDays.Tracks.Database;
using FWDays.Tracks.Loaders;

namespace FWDays.Tracks;

[ExtendObjectType(OperationTypeNames.Query)]
public class Queries
{
    [UseDbContext(typeof(TracksDbContext))]
    [UsePaging]
    public IQueryable<Track> GetTracks([Service] TracksDbContext context) 
        => context.Tracks.OrderBy(t => t.Name);
    
    public static Task<Track> GetTracksByIdAsync(int id, TracksByIdDataLoader tracksById, CancellationToken cancellationToken)
        => tracksById.LoadAsync(id, cancellationToken);
}