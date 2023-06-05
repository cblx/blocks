namespace Cblx.Blocks.Json.Tests.FlattenRootAttributeUsageWithFluent;
[FlattenRoot<PersonConfiguration>]
public class Person
{
    public required string Name { get; set; }
    [Flatten]
    public required Address Address { get; set; }
}
