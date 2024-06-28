using FWDays.Tracks;
using FWDays.Tracks.Database;
using FWDays.Tracks.Loaders;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(o =>
    o.AddDefaultPolicy(b =>
        b.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin()));

builder.Services
    .AddPooledDbContextFactory<TracksDbContext>(
        (s, o) => o
            .UseNpgsql(builder.Configuration.GetConnectionString("Database"))
            .UseLoggerFactory(s.GetRequiredService<ILoggerFactory>()));

builder.Services.AddScoped(serviceProvider => serviceProvider
    .GetRequiredService<IDbContextFactory<TracksDbContext>>()
    .CreateDbContext());

builder.Services.AddGraphQLServer()
    .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true)
    .AddQueryType()
    .AddMutationType()
    .AddTypeExtension<Queries>()
    .AddTypeExtension<Mutations>()
    .AddTypeExtension<TrackNode>()
    .AddDataLoader<TracksByIdDataLoader>()
    .AddFiltering()
    .AddSorting()
    .AddGlobalObjectIdentification()
    .EnsureDatabaseIsCreated();

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