using System.Text.Json;

namespace Cblx.Blocks.Json.Tests.FluentInheritance;

public class Tests
{
    private const string _jsonSubject  = """
        {
          "Name": "Mary",
          "this_street": "Elm Street",
          "zip_code": "12345"
        }
        """;

    private readonly Person _personSubject = new ()
    {
        Name = "Mary",
        Address = new Address
        {
            Street = "Elm Street",
            ZipCode = "12345"
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
