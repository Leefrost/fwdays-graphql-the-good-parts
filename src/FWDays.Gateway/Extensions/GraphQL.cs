using StackExchange.Redis;

namespace FWDays.Gateway.Extensions;

internal static class GraphQL
{
    public static IServiceCollection AddGraphQLSupport(this IServiceCollection services, IConfiguration configuration)
    {
        var graphQlConfiguration = new GraphQLConfiguration();
        configuration
            .GetSection(GraphQLConfiguration.SectionName)
            .Bind(graphQlConfiguration);

        foreach (var service in graphQlConfiguration.Services)
        {
            services.AddHttpClient(service.Name, c => c.BaseAddress = new Uri(service.Url));
        }

        services
            .AddSingleton(ConnectionMultiplexer.Connect(graphQlConfiguration.Redis!))
            .AddGraphQLServer()
            .ModifyRequestOptions(request => request.IncludeExceptionDetails = graphQlConfiguration.DetailedErrors)
            .InitializeOnStartup()
            .AddRemoteSchemasFromRedis(graphQlConfiguration.ServiceName!,
                sp => sp.GetRequiredService<ConnectionMultiplexer>());

        return services;
    }
}