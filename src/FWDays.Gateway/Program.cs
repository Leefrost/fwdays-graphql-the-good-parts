using FWDays.Gateway.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(o =>
    o.AddDefaultPolicy(b =>
        b.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin()));

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