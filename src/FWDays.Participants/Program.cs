using FWDays.Participants.Database;
using FWDays.Participants.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(o =>
    o.AddDefaultPolicy(b =>
        b.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin()));

builder.Services
    .AddPooledDbContextFactory<ParticipantsDbContext>(
        (s, o) => o
            .UseNpgsql(builder.Configuration.GetConnectionString("Database"))
            .UseLoggerFactory(s.GetRequiredService<ILoggerFactory>()));

builder.Services.AddScoped(serviceProvider => serviceProvider
    .GetRequiredService<IDbContextFactory<ParticipantsDbContext>>()
    .CreateDbContext());

builder.Services.SetupGraphQL(builder.Configuration);

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