using System.Text.Json;

namespace Cblx.Blocks.Json.Tests.Nested;

public class Tests
{
    private const string _jsonSubject  = """
        {
          "Name": "Mary",
          "Street": "Elm Street",
          "CityName": "S\u00E3o Paulo",
          "Country": "Brazil"
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
                CityName = "São Paulo",
                Country = "Brazil"
            }
        }
    };

    [Fact]
    public void CheckSimpleSerializationWithDiacritics()
    {
        string str = "São Paulo";
        Assert.Equal("""
            "S\u00E3o Paulo"
            """, JsonSerializer.Serialize(str));
    }

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
