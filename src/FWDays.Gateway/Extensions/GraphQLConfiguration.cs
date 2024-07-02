namespace FWDays.Gateway.Extensions;

internal record GraphQLServer(string Name, string Url);

internal class GraphQLConfiguration
{
    public const string SectionName = "GraphQL";
    
    public string? ServiceName { get; set; }
    public string? Redis { get; set; }
    public bool DetailedErrors { get; set; }
    public List<GraphQLServer> Services { get; set; } = [];
}