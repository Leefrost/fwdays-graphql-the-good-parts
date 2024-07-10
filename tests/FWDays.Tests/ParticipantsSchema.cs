using FWDays.Participants.Database;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using HotChocolate.Execution;
using Snapshooter.Xunit;

using Mutations = FWDays.Participants.Mutations;
using Queries = FWDays.Participants.Queries;

namespace FWDays.Tests;

public class ParticipantsSchema
{
    [Fact]
    public async Task SchemaChanged()
    {
        var builder = new ServiceCollection()
            .AddDbContextPool<ParticipantsDbContext>(
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