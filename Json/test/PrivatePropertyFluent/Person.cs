using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace Cblx.Blocks.Json.Tests.PrivatePropertyFluent;
[FlattenJsonRoot<PersonConfiguration>]
public class Person
{
    [JsonPropertyName("name")]
    public string Name { get; private set; } = "Mary";
    [FlattenJsonProperty]
    private Address Address { get; set; } = new();
    public Address GetAddress() => Address;

    public static Expression<Func<Person, object?>> GetAddressExpression() => p => p.Address;
}
