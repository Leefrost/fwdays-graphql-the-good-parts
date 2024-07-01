using FWDays.Tracks.Database;
using FWDays.Tracks.Loaders;

namespace FWDays.Tracks;

[Node]
[ExtendObjectType(typeof(Track))]
public class TrackNode
{
    [NodeResolver]
    public static Task<Track> GetTrackByIdAsync(int id, TracksByIdDataLoader tracksById, CancellationToken cancellationToken)
        => tracksById.LoadAsync(id, cancellationToken);
}