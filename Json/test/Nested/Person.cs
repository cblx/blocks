namespace Cblx.Blocks.Json.Tests.Nested;

public class Person
{
    public required string Name { get; set; }
    [Flatten]
    public required Address Address { get; set; }
}
