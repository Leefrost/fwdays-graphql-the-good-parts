using FWDays.Participants.Database;
using Microsoft.EntityFrameworkCore;

namespace FWDays.Participants.Loaders;

public class ParticipantsByIdDataLoader : BatchDataLoader<int, Participant>
{
    private readonly IDbContextFactory<ParticipantsDbContext> _dbContextFactory;

    public ParticipantsByIdDataLoader(
        IDbContextFactory<ParticipantsDbContext> dbContextFactory,
        IBatchScheduler batchScheduler,
        DataLoaderOptions options)
        : base(batchScheduler, options)
    {
        _dbContextFactory = dbContextFactory ?? 
                            throw new ArgumentNullException(nameof(dbContextFactory));
    }

    protected override async Task<IReadOnlyDictionary<int, Participant>> LoadBatchAsync(
        IReadOnlyList<int> keys, 
        CancellationToken cancellationToken)
    {
        await using ParticipantsDbContext dbContext = 
            _dbContextFactory.CreateDbContext();

        return await dbContext.Participants
            .Where(s => keys.Contains(s.Id))
            .ToDictionaryAsync(t => t.Id, cancellationToken);
    }
}