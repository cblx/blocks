namespace Cblx.Blocks.Json.Tests.FluentNested;

public class AddressConfiguration : FlattenJsonConfiguration<Address>
{
    public AddressConfiguration()
    {
        HasJsonPropertyName(a => a.Street, "street");
    }
}
