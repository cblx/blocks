namespace Cblx.Blocks.Json.Tests.FluentIgnore;

public class AddressConfiguration : FlattenJsonConfiguration<Address>
{
    public AddressConfiguration()
    {
        HasJsonPropertyName(a => a.Street, "this_street");
        Ignore(a => a.Ignored);
    }
}
