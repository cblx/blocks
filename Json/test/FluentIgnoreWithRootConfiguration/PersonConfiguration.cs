namespace Cblx.Blocks.Json.Tests.FluentIgnoreWithRootConfiguration;

public class PersonConfiguration : FlattenJsonConfiguration<Person>
{
    public PersonConfiguration()
    {
        HasJsonPropertyName(p => p.Name, "person_name");
    }
}