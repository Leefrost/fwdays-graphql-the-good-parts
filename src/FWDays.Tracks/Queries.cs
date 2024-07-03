using FWDays.Tracks.Database;
using FWDays.Tracks.Processing;
using Microsoft.EntityFrameworkCore;

namespace FWDays.Tracks;

[ExtendObjectType(OperationTypeNames.Query)]
public class Queries
{
    [UseDbContext(typeof(TracksDbContext))]
    [UsePaging]
    public IQueryable<Track> GetTracks([Service] TracksDbContext context) 
        => context.Tracks
            .Include(x=>x.Speaker)
            .Include(x=>x.Participants)
            .OrderBy(t => t.Topic);
    
    public static Task<Track> GetTracksByIdAsync(int id, TracksByIdDataLoader tracksById, CancellationToken cancellationToken)
        => tracksById.LoadAsync(id, cancellationToken);
}