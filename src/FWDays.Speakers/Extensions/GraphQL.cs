using FWDays.Speakers.Loaders;
using HotChocolate.Execution.Configuration;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace FWDays.Speakers.Extensions;

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
            .AddTypeExtension<SpeakerNode>()
            .AddDataLoader<SpeakerByIdDataLoader>()
            .AddFiltering()
            .AddSorting()
            .AddGlobalObjectIdentification()
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