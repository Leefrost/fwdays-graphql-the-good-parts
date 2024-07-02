using FWDays.Participants.Database;
using FWDays.Participants.Processing;
using HotChocolate.Execution.Configuration;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace FWDays.Participants.Extensions;

internal static class GraphQL
{
    public static IServiceCollection SetupGraphQL(this IServiceCollection services, IConfiguration configuration)
    {
        var graphQLConfiguration = new GraphQLConfiguration();
        configuration
            .GetSection(GraphQLConfiguration.SectionName)
            .Bind(graphQLConfiguration);

        services
            .AddSingleton(ConnectionMultiplexer.Connect(graphQLConfiguration.Redis!));

        services.AddGraphQLServer()
            .ModifyRequestOptions(
                opt => opt.IncludeExceptionDetails = graphQLConfiguration.DetailedErrors)
            .AddQueryType()
            .AddMutationType()
            .AddTypeExtension<Queries>()
            .AddTypeExtension<Mutations>()
            .AddDataLoader<ParticipantsByIdDataLoader>()
            .AddFiltering()
            .AddSorting()
            .EnsureDatabaseIsCreated()
            .PublishScheme(graphQLConfiguration);

        return services;
    }

    private static IRequestExecutorBuilder PublishScheme(
        this IRequestExecutorBuilder builder, GraphQLConfiguration graphQlConfiguration
        )
    {
        if (graphQlConfiguration.Federation)
        {
            builder.PublishSchemaDefinition(c => c
                .SetName(graphQlConfiguration.ServiceName!)
                .PublishToRedis(graphQlConfiguration.GatewayName!, 
                    sp => sp.GetRequiredService<ConnectionMultiplexer>()));
        }

        return builder;
    }

    private static IRequestExecutorBuilder EnsureDatabaseIsCreated(this IRequestExecutorBuilder builder) =>
        builder.ConfigureSchemaAsync(async (services, _, cancellationToken) =>
        {
            var factory = services.GetRequiredService<IDbContextFactory<ParticipantsDbContext>>();
            await using var dbContext = factory.CreateDbContext();

            if (await dbContext.Database.EnsureCreatedAsync(cancellationToken))
            {
                dbContext.Participants.Add(new Participant
                {
                    FirstName = "Michael",
                    LastName = "Staib",
                    EmailAddress = "michael@chillicream.com"
                });
            }

            await dbContext.SaveChangesAsync(cancellationToken);
        });
}