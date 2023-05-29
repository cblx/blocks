namespace Cblx.Blocks.Json.Tests.Simple;

public class Person
{
    public required string Name { get; set; }
    public required int Age { get; set; }
    [Flatten]
    public required Address Address { get; set; }
}
