namespace Cblx.Blocks.Json.Tests.FluentIgnoreWithRootConfiguration;

public class AddressConfiguration : FlattenJsonConfiguration<AddressVo>
{
    public AddressConfiguration()
    {
        HasJsonPropertyName(a => a.Street, "this_street");
        HasJsonPropertyName(AddressVo.AddressMember, "internal");
        Ignore(a => a.Ignored);
    }
}
