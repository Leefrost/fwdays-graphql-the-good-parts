using Microsoft.EntityFrameworkCore;

namespace FWDays.Speakers.Loaders;

public class SpeakerByIdDataLoader : BatchDataLoader<int, Speaker>
{
    private readonly IDbContextFactory<SpeakersDbContext> _dbContextFactory;

    public SpeakerByIdDataLoader(
        IDbContextFactory<SpeakersDbContext> dbContextFactory,
        IBatchScheduler batchScheduler,
        DataLoaderOptions options)
        : base(batchScheduler, options)
    {
        _dbContextFactory = dbContextFactory ?? 
                            throw new ArgumentNullException(nameof(dbContextFactory));
    }

    protected override async Task<IReadOnlyDictionary<int, Speaker>> LoadBatchAsync(
        IReadOnlyList<int> keys, 
        CancellationToken cancellationToken)
    {
        await using SpeakersDbContext dbContext = 
            _dbContextFactory.CreateDbContext();

        return await dbContext.Speakers
            .Where(s => keys.Contains(s.Id))
            .ToDictionaryAsync(t => t.Id, cancellationToken);
    }
}