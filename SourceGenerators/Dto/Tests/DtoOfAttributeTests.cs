using Cblx.Blocks.SourceGenerators.Dto.Tests.Aux;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
namespace Cblx.Blocks.SourceGenerators.Dto.Tests;
public class DtoOfAttributeTests
{
    [Fact]
    public void Entity1ToDtoTest()
    {
        var code = """
            #nullable enable
            using Cblx.Blocks;
            using Cblx.Blocks.SourceGenerators.Dto.Tests.Aux;
            namespace MyNamespace;
            [DtoOf<Entity1>]
            [DtoIgnore(Property = nameof(Entity1.IgnoreMe))]
            public partial class Entity1Dto {}
            """;
        var generated = """
            #nullable enable
            namespace MyNamespace;
            partial class Entity1Dto
            {
                public string? Name { get; set; }
                public int? Age { get; set; }
                public System.Guid? Id { get; set; }
            }

            """;
        TestHelpers.AssertGeneration<DtoAndMapperGenerator>(code, generated, "DtoExtraction", additionalReferences: [typeof(Entity1).Assembly]);
    }
    [Fact]
    public void Entity1ToDtoWithFromMapper()
    {
        var code = """
            #nullable enable
            using Cblx.Blocks;
            using Cblx.Blocks.SourceGenerators.Dto.Tests.Aux;
            namespace MyNamespace;
            [DtoOf<Entity1>]
            [DtoIgnore(Property = nameof(Entity1.IgnoreMe))]
            public partial class Entity1Dto 
            {
                [Mapper] internal static partial Entity1Dto From(Entity1 entity);
            }
            """;
        var generated = """
            #nullable enable
            namespace MyNamespace;
            partial class Entity1Dto
            {
                public string? Name { get; set; }
                public int? Age { get; set; }
                public System.Guid? Id { get; set; }
                internal static partial MyNamespace.Entity1Dto From(Cblx.Blocks.SourceGenerators.Dto.Tests.Aux.Entity1 entity)
                {
                    return new MyNamespace.Entity1Dto()
                    {
                        Name = entity.Name,
                        Age = entity.Age,
                        Id = entity.Id,
                    };
                }
            }

            """;
        TestHelpers.AssertGeneration<DtoAndMapperGenerator>(code, generated, "DtoExtraction", additionalReferences: [typeof(Entity1).Assembly]);
    }
}