namespace Cblx.Blocks.Json.Tests.FlattenRootAttributeUsage;
[FlattenRoot]
public class Person
{
    public required string Name { get; set; }
    [Flatten]
    public required Address Address { get; set; }
}
