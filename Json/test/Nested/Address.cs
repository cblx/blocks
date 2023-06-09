namespace Cblx.Blocks.Json.Tests.Nested;

public class Address
{
    public required string Street { get; set; }
    [FlattenJsonProperty]
    public required City City { get; set; }
}
