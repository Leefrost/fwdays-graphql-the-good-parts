using HotChocolate.Execution.Configuration;
using Microsoft.EntityFrameworkCore;

namespace FWDays.Speakers;

public static class GraphQLExtensions
{
    public static IRequestExecutorBuilder EnsureDatabaseIsCreated(this IRequestExecutorBuilder builder) =>
        builder.ConfigureSchemaAsync(async (services, _, cancellationToken) =>
        {
            var factory = services.GetRequiredService<IDbContextFactory<SpeakersDbContext>>();
            await using var dbContext = factory.CreateDbContext();

            if (await dbContext.Database.EnsureCreatedAsync(cancellationToken))
            {
                dbContext.Speakers.Add(new Speaker
                {
                    Name = "Sergii Lischuk",
                    Bio = ".NET Developer",
                    Company = "Leobit"
                });
            }

            await dbContext.SaveChangesAsync(cancellationToken);
        });
}