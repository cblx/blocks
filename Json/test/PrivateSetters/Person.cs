using System.Text.Json.Serialization;

namespace Cblx.Blocks.Json.Tests.PrivateSetters;

public class Person
{
    [JsonPropertyName("name")]
    public string Name { get; private set; } = "Mary";
    [Flatten]
    public Address Address { get; private set; } = new();
}
