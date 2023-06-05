namespace Cblx.Blocks.Json.Tests.FlattenRootAttributeUsageWithFluent;

public class PersonConfiguration : FlattenJsonConfiguration<Person>
{
    public PersonConfiguration()
    {
        HasJsonPropertyName(x => x.Name, "ma_name");
    }
}   