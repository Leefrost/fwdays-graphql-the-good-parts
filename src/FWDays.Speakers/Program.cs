using FWDays.Speakers;
using FWDays.Speakers.Loaders;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(o =>
                    o.AddDefaultPolicy(b =>
                        b.AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowAnyOrigin()));

builder.Services
    .AddPooledDbContextFactory<SpeakersDbContext>(
    (s, o) => o
        .UseNpgsql(builder.Configuration.GetConnectionString("Database"))
        .UseLoggerFactory(s.GetRequiredService<ILoggerFactory>()));

builder.Services.AddScoped(serviceProvider => serviceProvider
    .GetRequiredService<IDbContextFactory<SpeakersDbContext>>()
    .CreateDbContext());

builder.Services.AddGraphQLServer()
    .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true)
    .AddQueryType()
    .AddMutationType()
    .AddTypeExtension<Queries>()
    .AddTypeExtension<Mutations>()
    .AddTypeExtension<SpeakerNode>()
    .AddDataLoader<SpeakerByIdDataLoader>()
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