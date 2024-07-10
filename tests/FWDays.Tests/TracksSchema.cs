using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using HotChocolate.Execution;
using Snapshooter.Xunit;
using FWDays.Tracks.Database;

using Mutations = FWDays.Tracks.Mutations;
using Queries = FWDays.Tracks.Queries;

namespace FWDays.Tests;

public class TracksSchema
{
    [Fact]
    public async Task SchemaChanged()
    {
        var builder = new ServiceCollection()
            .AddDbContextPool<TracksDbContext>(
                options => options.UseInMemoryDatabase("Data Source=fwdays.db"))
            .AddGraphQL()
            .AddQueryType(d => d.Name("Query"))
            .AddTypeExtension<Queries>()
            .AddMutationType(d => d.Name("Mutation"))
            .AddTypeExtension<Mutations>();
        
        var schema = await builder.BuildSchemaAsync();

        schema.Print().MatchSnapshot();
    }
}