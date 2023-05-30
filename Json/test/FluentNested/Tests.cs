using System.Text.Json;

namespace Cblx.Blocks.Json.Tests.FluentNested;

public class Tests
{
    private const string _jsonSubject  = """
        {
          "Name": "Mary",
          "street": "Elm Street",
          "city": "Rio",
          "country": "Brazil"
        }
        """;

    private readonly Person _personSubject = new Person
    {
        Name = "Mary",
        Address = new Address
        {
            Street = "Elm Street",
            City = new City
            {
                CityName = "Rio",
                Country = "Brazil"
            }
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
