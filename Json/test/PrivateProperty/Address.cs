using System.Text.Json.Serialization;

namespace Cblx.Blocks.Json.Tests.PrivateProperty;

public class Address
{
    [JsonPropertyName("person_street")]
    public string Street { get; private set; } = "Elm Street";
}