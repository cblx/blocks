using Cblx.Blocks.SourceGenerators.Dto.Tests.Aux;
namespace Cblx.Blocks.SourceGenerators.Dto.Tests;
public class MapperAttributeTests
{
    [Fact]
    public void Entity1DtoStaticFromTest()
    {
        var code = """
            #nullable enable
            using Cblx.Blocks;
            using Cblx.Blocks.SourceGenerators.Dto.Tests.Aux;
            namespace MyNamespace;
            public partial class Entity1Dto 
            {
                public string? Name { get; set; }
                public int? Age { get; set; }

                [Mapper]
                internal static partial Entity1Dto From(Entity1 entity);
            }
            """;
        var generated = """
            #nullable enable
            namespace MyNamespace;
            partial class Entity1Dto
            {
                internal static partial MyNamespace.Entity1Dto From(Cblx.Blocks.SourceGenerators.Dto.Tests.Aux.Entity1 entity)
                {
                    return new MyNamespace.Entity1Dto()
                    {
                        Name = entity.Name,
                        Age = entity.Age,
                    };
                }
            }

            """;
        //TestHelpers.AssertGeneration<MapperGenerator>(code, generated, additionalReferences: [typeof(Entity1).Assembly]);
        TestHelpers.AssertGeneration<DtoAndMapperGenerator>(code, generated, "MapperExtraction", additionalReferences: [typeof(Entity1).Assembly]);
    }

    [Fact]
    public void Entity1DtoStaticFromDefaultConstructorTest()
    {
        var code = """
            #nullable enable
            using Cblx.Blocks;
            using Cblx.Blocks.SourceGenerators.Dto.Tests.Aux;
            namespace MyNamespace;
            public partial class Entity1Dto(string name, int age) 
            {
                public string? Name { get; set; } = name;
                public int? Age { get; private set; } = age;

                [Mapper]
                internal static partial Entity1Dto From(Entity1 entity);
            }
            """;
        var generated = """
            #nullable enable
            namespace MyNamespace;
            partial class Entity1Dto
            {
                internal static partial MyNamespace.Entity1Dto From(Cblx.Blocks.SourceGenerators.Dto.Tests.Aux.Entity1 entity)
                {
                    return new MyNamespace.Entity1Dto(entity.Name, entity.Age)
                    {
                        Name = entity.Name,
                    };
                }
            }

            """;
        //        TestHelpers.AssertGeneration<MapperGenerator>(code, generated, additionalReferences: [typeof(Entity1).Assembly]);
        TestHelpers.AssertGeneration<DtoAndMapperGenerator>(code, generated, "MapperExtraction", additionalReferences: [typeof(Entity1).Assembly]);
    }

    [Fact]
    public void Entity1DtoToEntity1()
    {
        var code = """
            #nullable enable
            using Cblx.Blocks;
            using Cblx.Blocks.SourceGenerators.Dto.Tests.Aux;
            namespace MyNamespace;
            public partial class Entity1Dto(string name, int age) 
            {
                public string? Name { get; set; } = name;
                public int? Age { get; private set; } = age;

                [Mapper]
                internal partial Entity1 ToEntity();
            }
            """;
        var generated = """
            #nullable enable
            namespace MyNamespace;
            partial class Entity1Dto
            {
                internal partial Cblx.Blocks.SourceGenerators.Dto.Tests.Aux.Entity1 ToEntity()
                {
                    return new Cblx.Blocks.SourceGenerators.Dto.Tests.Aux.Entity1(this.Name!, this.Age.GetValueOrDefault());
                }
            }

            """;
        //TestHelpers.AssertGeneration<MapperGenerator>(code, generated, additionalReferences: [typeof(Entity1).Assembly]);
        TestHelpers.AssertGeneration<DtoAndMapperGenerator>(code, generated, "MapperExtraction", additionalReferences: [typeof(Entity1).Assembly]);
    }

    [Fact]
    public void NullabilityDivergenceTest()
    {
        var code = """
            #nullable enable
            using Cblx.Blocks;
            namespace MyNamespace;
            public partial class Dto1
            {
                public string? Name { get; set; }
                public int? Age { get; private set; }

                [Mapper]
                internal partial Dto2 ToDto2();
            }

            public class Dto2
            {
                public required string Name { get; set; }
                public required int Age { get; set; }
            }
            """;
        var generated = """
            #nullable enable
            namespace MyNamespace;
            partial class Dto1
            {
                internal partial MyNamespace.Dto2 ToDto2()
                {
                    return new MyNamespace.Dto2()
                    {
                        Name = this.Name!,
                        Age = this.Age.GetValueOrDefault(),
                    };
                }
            }

            """;
        //TestHelpers.AssertGeneration<MapperGenerator>(code, generated);
        TestHelpers.AssertGeneration<DtoAndMapperGenerator>(code, generated, "MapperExtraction");
    }
}
