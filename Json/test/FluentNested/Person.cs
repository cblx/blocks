namespace Cblx.Blocks.Json.Tests.FluentNested;

public class Person
{
    public required string Name { get; set; }
    [FlattenJsonProperty<AddressConfiguration>]
    public required Address Address { get; set; }
}
