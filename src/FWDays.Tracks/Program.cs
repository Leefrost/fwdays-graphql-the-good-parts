using FWDays.Tracks.Database;
using FWDays.Tracks.Extensions;
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

builder.Services.AddGraphQLSupport(builder.Configuration);

var app = builder.Build();

app.UseCors();
app.UseRouting();
app.MapGraphQL();
app.MapHealthChecks("/health");

app.MapGet("/", context =>
{
    context.Response.Redirect("/graphql", true);
    return Task.CompletedTask;
});

app.Run();