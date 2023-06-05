using System.Text.Json;

namespace Cblx.Blocks.Json.Tests.FlattenRootAttributeUsage;

public class Tests
{
    private const string _jsonSubject  = """
        {
          "Name": "Mary",
          "Street": "Elm Street"
        }
        """;

    private readonly Person _personSubject = new Person
    {
        Name = "Mary",
        Address = new Address
        {
            Street = "Elm Street"
        }
    };

    [Fact]
    public void Serializing()
    {
        Assert.Equal(_jsonSubject, JsonSerializer.Serialize(_personSubject, new JsonSerializerOptions { WriteIndented = true }));
    }

    [Fact]
    public void Deserializing()
    {
        Assert.Equivalent(_personSubject, JsonSerializer.Deserialize<Person>(_jsonSubject, new JsonSerializerOptions { WriteIndented = true }));
    }
}
