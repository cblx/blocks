using System.Text.Json;

namespace Cblx.Blocks.Json.Tests.FluentTwoFlattenPropsForSameType;

public class Tests
{
    private const string _jsonSubject  = """
        {
          "Name": "Mary",
          "this_street": "Elm Street",
          "comercial_street": "Other Street"
        }
        """;

    private readonly Person _personSubject = new ()
    {
        Name = "Mary",
        Address = new Address
        {
            Street = "Elm Street"
        },
        ComercialAddress = new Address
        {
            Street = "Other Street"
        }
    };

    [Fact]
    public void Serializing()
    {
        Assert.Equal(_jsonSubject, JsonSerializer.Serialize(_personSubject, Fixtures.CreateOptions<Person>()));
    }

    [Fact]
    public void Deserializing()
    {
        Assert.Equivalent(_personSubject, JsonSerializer.Deserialize<Person>(_jsonSubject, Fixtures.CreateOptions<Person>()));
    }
}
