using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(o =>
    o.AddDefaultPolicy(b =>
        b.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin()));

const string participants = "participants";
const string speakers = "speakers";
const string tracks = "tracks";

var participantsUrl = builder.Configuration.GetValue<string>("ParticipantsApi");
var speakersUrl = builder.Configuration.GetValue<string>("SpeakersApi");
var tracksUrl = builder.Configuration.GetValue<string>("TracksApi");

var schemaConnString = builder.Configuration.GetConnectionString("Redis");

builder.Services.AddHttpClient(participants, c => c.BaseAddress = new Uri(participantsUrl!));
builder.Services.AddHttpClient(speakers, c => c.BaseAddress = new Uri(speakersUrl!));
builder.Services.AddHttpClient(tracks, c => c.BaseAddress = new Uri(tracksUrl!));

builder.Services.AddSingleton(ConnectionMultiplexer.Connect(schemaConnString!));

builder.Services
    .AddGraphQLServer()
    .AddQueryType(d => d.Name("Query"))
    .AddRemoteSchemasFromRedis("FWDays", sp => sp.GetRequiredService<ConnectionMultiplexer>());
    
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