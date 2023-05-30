using System.Text.Json;

namespace Cblx.Blocks.Json.Tests.AttributeUsage2;

public class Tests
{
    private const string _jsonSubject  = """
        {
          "Name": "Mary",
          "Age": 18,
          "Description": "desc",
          "Street": "Elm Street",
          "Number": 123,
          "District": "dist"
        }
        """;

    private readonly Person _personSubject = new Person
    {
        Name = "Mary",
        Age = 18,
        Description = "desc",
        Address = new Address
        {
            Street = "Elm Street",
            Number = 123,
            District = "dist"
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
