namespace Cblx.Blocks.Json.Tests.FluentTwoFlattenPropsForSameType;

public class AddressComercialConfiguration : FlattenJsonConfiguration<Address>
{
    public AddressComercialConfiguration()
    {
        HasJsonPropertyName(a => a.Street, "comercial_street");
    }
}
