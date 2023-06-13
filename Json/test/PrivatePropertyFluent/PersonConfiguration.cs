namespace Cblx.Blocks.Json.Tests.PrivatePropertyFluent;

public class PersonConfiguration : FlattenJsonConfiguration<Person>
{
    public PersonConfiguration()
    {
        //IncludePrivateProperty("Address");
        IncludePrivateProperty(Person.GetAddressExpression());
    }
}