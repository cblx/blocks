namespace Cblx.Blocks.Json.Tests.Fluent;

public class AddressConfiguration : FlattenJsonConfiguration<Address>
{
    public AddressConfiguration()
    {
        HasJsonPropertyName(a => a.Street, "this_street");
    }
}
