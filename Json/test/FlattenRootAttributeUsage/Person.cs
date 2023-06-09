namespace Cblx.Blocks.Json.Tests.FlattenRootAttributeUsage;
[FlattenJsonRoot]
public class Person
{
    public required string Name { get; set; }
    [FlattenJsonProperty]
    public required Address Address { get; set; }
}
