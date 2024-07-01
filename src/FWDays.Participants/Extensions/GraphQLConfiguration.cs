namespace FWDays.Participants.Extensions;

public class GraphQLConfiguration
{
    public const string SectionName = "GraphQL";
    
    public string? GatewayName { get; set; }
    public string? ServiceName { get; set; }
    public string? Redis { get; set; }
    public bool Federation { get; set; }
    public bool DetailedErrors { get; set; }
}