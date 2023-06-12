namespace Cblx.Blocks.Json.Tests.FluentInheritance;

public class AddressConfiguration : FlattenJsonConfiguration<Address>
{
    public AddressConfiguration()
    {
        HasJsonPropertyName(a => a.Street, "this_street");
        HasJsonPropertyName(a => a.ZipCode, "zip_code");
    }
}
