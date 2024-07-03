using FWDays.Tracks.Database;
using Microsoft.EntityFrameworkCore;

namespace FWDays.Tracks.Processing;

public class TracksByIdDataLoader : BatchDataLoader<int, Track>
{
    private readonly IDbContextFactory<TracksDbContext> _dbContextFactory;

    public TracksByIdDataLoader(
        IDbContextFactory<TracksDbContext> dbContextFactory,
        IBatchScheduler batchScheduler,
        DataLoaderOptions options)
        : base(batchScheduler, options)
    {
        _dbContextFactory = dbContextFactory ?? 
                            throw new ArgumentNullException(nameof(dbContextFactory));
    }

    protected override async Task<IReadOnlyDictionary<int, Track>> LoadBatchAsync(
        IReadOnlyList<int> keys, 
        CancellationToken cancellationToken)
    {
        await using TracksDbContext dbContext = 
            _dbContextFactory.CreateDbContext();

        return await dbContext.Tracks
            .Where(s => keys.Contains(s.Id))
            .ToDictionaryAsync(t => t.Id, cancellationToken);
    }
}