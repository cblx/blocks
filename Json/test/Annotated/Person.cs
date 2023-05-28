using System.Text.Json.Serialization;

namespace Cblx.Blocks.Json.Tests.Annotated;

public class Person
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }
    [Flatten]
    public required Address Address { get; set; }
}
