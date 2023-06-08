using System.Text.Json.Serialization;

namespace Cblx.Blocks.Json.Tests.PrivateProperty;

public class Person
{
    [JsonPropertyName("name")]
    public string Name { get; private set; } = "Mary";
    [Flatten]
    private Address Address { get; set; } = new();
    public Address GetAddress() => Address;
}
