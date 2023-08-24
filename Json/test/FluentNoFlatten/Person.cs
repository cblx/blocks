using System.Text.Json.Serialization;

namespace Cblx.Blocks.Json.Tests.FluentNoFlatten;

public class Person
{
    public required string Name { get; set; }
    [FluentJsonProperty<AddressConfiguration>(Flatten = false)]
    [JsonPropertyName("address")]
    public required Address Address { get; set; }
}
