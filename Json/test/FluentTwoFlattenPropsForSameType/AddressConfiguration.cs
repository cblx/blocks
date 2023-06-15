namespace Cblx.Blocks.Json.Tests.FluentTwoFlattenPropsForSameType;

public class AddressConfiguration : FlattenJsonConfiguration<Address>
{
    public AddressConfiguration()
    {
        HasJsonPropertyName(a => a.Street, "this_street");
    }
}
