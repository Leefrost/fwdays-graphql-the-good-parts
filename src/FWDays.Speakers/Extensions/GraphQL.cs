using FWDays.Speakers.Processing;
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
            .AddDataLoader<SpeakerByIdDataLoader>()
            .AddFiltering()
            .AddSorting()
            .EnsureDatabaseIsCreated()
            .PublishScheme(graphQLConfiguration);
        
        if (graphQLConfiguration.Federation)
            services
                .AddHealthChecks()
                .AddRedis(graphQLConfiguration.Redis!);

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
            var factory = services.GetRequiredService<IDbContextFactory<SpeakersDbContext>>();
            await using var dbContext = factory.CreateDbContext();

            if (await dbContext.Database.EnsureCreatedAsync(cancellationToken))
            {
                dbContext.Speakers.Add(new Speaker
                {
                    FirstName = "Sergii",
                    LastName = "Lischuk",
                    Topic = "GraphQL: The Good parts",
                    Position = ".NET Developer",
                    Company = "Leobit"
                });
            }
            
            await dbContext.SaveChangesAsync(cancellationToken);

        });
}