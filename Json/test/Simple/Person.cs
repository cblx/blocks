namespace Cblx.Blocks.Json.Tests.Simple;

public class Person
{
    public required string Name { get; set; }
    [Flatten]
    public required Address Address { get; set; }
}
