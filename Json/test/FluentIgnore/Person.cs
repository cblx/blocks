namespace Cblx.Blocks.Json.Tests.FluentIgnore;

public class Person
{
    public required string Name { get; set; }
    [FlattenJsonProperty<AddressConfiguration>]
    public required Address Address { get; set; }
}
