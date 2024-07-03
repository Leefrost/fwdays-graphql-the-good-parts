using FWDays.Tracks.Database;
using FWDays.Tracks.Processing;
using HotChocolate.Execution.Configuration;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace FWDays.Tracks.Extensions;

internal static class GraphQL
{
    public static IServiceCollection AddGraphQLSupport(this IServiceCollection services, IConfiguration configuration)
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
            .AddDataLoader<TracksByIdDataLoader>()
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
        if (!graphQlConfiguration.Federation) 
            return builder;
        
        builder.PublishSchemaDefinition(c => c
            .SetName(graphQlConfiguration.ServiceName!)
            .PublishToRedis(graphQlConfiguration.GatewayName!, 
                sp => sp.GetRequiredService<ConnectionMultiplexer>()));
        builder.InitializeOnStartup();

        return builder;
    }

    private static IRequestExecutorBuilder EnsureDatabaseIsCreated(this IRequestExecutorBuilder builder) =>
        builder.ConfigureSchemaAsync(async (services, _, cancellationToken) =>
        {
            var factory = services.GetRequiredService<IDbContextFactory<TracksDbContext>>();
            await using var dbContext = await factory.CreateDbContextAsync(cancellationToken);

            if (await dbContext.Database.EnsureCreatedAsync(cancellationToken))
            {
                dbContext.Tracks.Add(new Track
                {
                    Topic = "GraphQL: The good parts",
                    Speaker = new TrackSpeaker {
                        FirstName = "Sergii",
                        LastName = "Lischuk"
                    },
                    Participants =
                    [
                        new TrackParticipant
                        {
                            FirstName = "Michael",
                            LastName = "Staib",

                        }
                    ]
                });
            }

            await dbContext.SaveChangesAsync(cancellationToken);
        });
        
}