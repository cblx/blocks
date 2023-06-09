using System.Text.Json.Serialization;

namespace Cblx.Blocks.Json.Tests.AttributeUsageAndNullValues;

[JsonConverter(typeof(FlattenJsonConverter<Person>))]
public class Person
{
    public string? Name { get; set; }
    public int Age { get; set; }
    public string? Description { get; set; }
    [FlattenJsonProperty]
    public Address Address { get; set; } = new();
}
