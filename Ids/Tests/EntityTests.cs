using System.Text.Json;

namespace Cblx.Blocks.Ids.Tests;

public class EntityTests
{
    private readonly JsonSerializerOptions _options = new() { WriteIndented = true };

    [Fact]
    public void ShouldTestClassEntity()
    {
        var entity = new Entity();

        Assert.Equal(Guid.Empty, entity.IdDefault);
        Assert.Equal(Guid.Empty, entity.IdEmpty);
        Assert.NotEqual(Guid.Empty, entity.IdValid);
        Assert.Null(entity.IdNullable);
    }

    [Fact]
    public void ShouldConvertEntityToJson()
    {
        var entity = new Entity();
        var json = JsonSerializer.Serialize(entity, _options);

        var expected = $$"""
                         {
                           "IdDefault": "00000000-0000-0000-0000-000000000000",
                           "IdNullable": null,
                           "IdEmpty": "00000000-0000-0000-0000-000000000000",
                           "IdValid": "{{entity.IdValid.Guid}}"
                         }
                         """;

        Assert.Equal(expected, json);
    }

    [Fact]
    public void ShouldConvertJsonToEntity()
    {
        const string json = """
                            {
                              "IdDefault": "00000000-0000-0000-0000-000000000000",
                              "IdNullable": null,
                              "IdEmpty": "00000000-0000-0000-0000-000000000000",
                              "IdValid": "24711044-3c62-431c-a824-938dabae34a1"
                            }
                            """;

        var entity = JsonSerializer.Deserialize<Entity>(json, _options)!;

        Assert.Equal(Guid.Empty, entity.IdDefault);
        Assert.Equal(Guid.Empty, entity.IdEmpty);
        Assert.NotEqual(Guid.Empty, entity.IdValid);
        Assert.Null(entity.IdNullable);
    }
}