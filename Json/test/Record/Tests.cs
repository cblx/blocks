using System.Text.Json;

namespace Cblx.Blocks.Json.Tests.Record;

public class Tests
{
    private const string _jsonSubject  = """
        {
          "Name": "Mary",
          "Age": 32,
          "Street": "Elm Street"
        }
        """;

    private readonly Person _personSubject = new Person
    {
        Name = "Mary",
        Age = 32,
        Address = new Address
        {
            Street = "Elm Street"
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
