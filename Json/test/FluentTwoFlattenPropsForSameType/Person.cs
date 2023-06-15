namespace Cblx.Blocks.Json.Tests.FluentTwoFlattenPropsForSameType;

public class Person
{
    public required string Name { get; set; }
    [FlattenJsonProperty<AddressConfiguration>]
    public required Address Address { get; set; }

    [FlattenJsonProperty<AddressComercialConfiguration>]
    public required Address ComercialAddress { get; set; }
}
