using FWDays.Tracks.Database;
using HotChocolate.Execution.Configuration;
using Microsoft.EntityFrameworkCore;

namespace FWDays.Tracks;

public static class GraphQLExtensions
{
    public static IRequestExecutorBuilder EnsureDatabaseIsCreated(this IRequestExecutorBuilder builder) =>
        builder.ConfigureSchemaAsync(async (services, _, cancellationToken) =>
        {
            var factory = services.GetRequiredService<IDbContextFactory<TracksDbContext>>();
            await using var dbContext = factory.CreateDbContext();

            if (await dbContext.Database.EnsureCreatedAsync(cancellationToken))
            {
                dbContext.Tracks.Add(new Track
                {
                    Name = "GraphQL: The good parts",
                });
            }

            await dbContext.SaveChangesAsync(cancellationToken);
        });
}