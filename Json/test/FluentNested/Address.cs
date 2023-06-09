namespace Cblx.Blocks.Json.Tests.FluentNested;

public class Address
{
    public required string Street { get; set; }
    [FlattenJsonProperty<CityConfiguration>]
    public required City City { get; set; }
}