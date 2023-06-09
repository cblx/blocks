using System.Text.Json.Serialization;

namespace Cblx.Blocks.Json.Tests.Annotated;

public class Person
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }
    [FlattenJsonProperty]
    public required Address Address { get; set; }
}
