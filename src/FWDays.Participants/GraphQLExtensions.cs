using FWDays.Participants.Database;
using HotChocolate.Execution.Configuration;
using Microsoft.EntityFrameworkCore;

namespace FWDays.Participants;

public static class GraphQLExtensions
{
    public static IRequestExecutorBuilder EnsureDatabaseIsCreated(this IRequestExecutorBuilder builder) =>
        builder.ConfigureSchemaAsync(async (services, _, cancellationToken) =>
        {
            var factory = services.GetRequiredService<IDbContextFactory<ParticipantsDbContext>>();
            await using var dbContext = factory.CreateDbContext();

            if (await dbContext.Database.EnsureCreatedAsync(cancellationToken))
            {
                dbContext.Participants.Add(new Participant
                {
                    FirstName = "Michael ",
                    LastName = "Staib",
                    EmailAddress = "michael@chillicream.com"
                });
            }

            await dbContext.SaveChangesAsync(cancellationToken);
        });
}