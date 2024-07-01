using FWDays.Gateway.Extensions;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(o =>
    o.AddDefaultPolicy(b =>
        b.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin()));

var graphQlConfig = builder.Configuration
    .GetSection(GraphQLConfiguration.SectionName)
    .Get<GraphQLConfiguration>();

builder.Services
    .AddGraphQLServices(graphQlConfig!);

builder.Services
    .AddSingleton(ConnectionMultiplexer.Connect(graphQlConfig!.Redis!))
    .AddGraphQLServer()
    .AddRemoteSchemasFromRedis(graphQlConfig.ServiceName!, sp => sp.GetRequiredService<ConnectionMultiplexer>());
    
var app = builder.Build();

app.UseCors();
app.UseRouting();
app.MapGraphQL();

app.MapGet("/", context =>
{
    context.Response.Redirect("/graphql", true);
    return Task.CompletedTask;
});

app.Run();