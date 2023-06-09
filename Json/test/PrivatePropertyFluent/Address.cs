using System.Text.Json.Serialization;

namespace Cblx.Blocks.Json.Tests.PrivatePropertyFluent;

public class Address
{
    [JsonPropertyName("person_street")]
    public string Street { get; private set; } = "Elm Street";
}