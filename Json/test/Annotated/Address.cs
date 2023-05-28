using System.Text.Json.Serialization;

namespace Cblx.Blocks.Json.Tests.Annotated;

public class Address
{
    [JsonPropertyName("person_street")]
    public required string Street { get; set; }
}