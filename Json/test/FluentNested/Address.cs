namespace Cblx.Blocks.Json.Tests.FluentNested;

public class Address
{
    public required string Street { get; set; }
    [Flatten<CityConfiguration>]
    public required City City { get; set; }
}