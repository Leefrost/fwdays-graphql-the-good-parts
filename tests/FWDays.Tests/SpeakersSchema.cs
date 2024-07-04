using FWDays.Speakers;
using HotChocolate;
using HotChocolate.Execution;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Snapshooter.Xunit;

using Mutations = FWDays.Speakers.Mutations;
using Queries = FWDays.Speakers.Queries;

namespace FWDays.Tests;

public class SpeakersSchema
{
    [Fact]
    public async Task SchemaChanged()
    {
        ISchema schema =
            await new ServiceCollection()
                .AddDbContextPool<SpeakersDbContext>(
                    options => options.UseInMemoryDatabase("Data Source=fwdays.db"))
                .AddGraphQL()
            .AddQueryType(d => d.Name("Query"))
                .AddTypeExtension<Queries>()
            .AddMutationType(d => d.Name("Mutation"))
                .AddTypeExtension<Mutations>()
            .BuildSchemaAsync();

        schema.Print().MatchSnapshot();
    }
}