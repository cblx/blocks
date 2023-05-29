namespace Cblx.Blocks.Json.Tests.Fluent;

public class Person
{
    public required string Name { get; set; }
    [Flatten<AddressConfiguration>]
    public required Address Address { get; set; }
}
