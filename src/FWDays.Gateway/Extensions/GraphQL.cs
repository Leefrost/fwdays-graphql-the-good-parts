namespace FWDays.Gateway.Extensions;

internal static class GraphQL
{
    public static IServiceCollection AddGraphQLServices(this IServiceCollection services, GraphQLConfiguration graphQlConfiguration)
    {
        foreach (var service in graphQlConfiguration.Services)
        {
            services.AddHttpClient(service.Name, c => c.BaseAddress = new Uri(service.Url));
        }

        return services;
    }
}