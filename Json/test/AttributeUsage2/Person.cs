using System.Text.Json.Serialization;

namespace Cblx.Blocks.Json.Tests.AttributeUsage2;

[JsonConverter(typeof(FlattenJsonConverter<Person>))]
public class Person
{
    public required string Name { get; set; }
    public required int Age { get; set; }
    public required string Description { get; set; }
    [FlattenJsonProperty]
    public required Address Address { get; set; }
}
