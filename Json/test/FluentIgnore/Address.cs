namespace Cblx.Blocks.Json.Tests.FluentIgnore;

public class Address
{
    public required string Street { get; set; }

    public required string? Ignored { get; set; }
}