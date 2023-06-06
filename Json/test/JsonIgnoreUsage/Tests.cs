using System.Text.Json;

namespace Cblx.Blocks.Json.Tests.JsonIgnoreUsage;

public class Tests
{
    private const string _jsonSubjectForSerialization  = """
        {
          "Name": "Mary",
          "Age": 32,
          "Street": "Elm Street"
        }
        """;

  
    private readonly Person _personSubjectForSerialization = new ()
    {
        Name = "Mary",
        Age = 32,
        InternalInfo = "This should not be serialized",
        Address = new Address
        {
            Street = "Elm Street"
        }
    };

    private const string _jsonSubjectForDeserialization = """
        {
          "Name": "Mary",
          "Age": 32,
          "Street": "Elm Street",
          "InternalInfo": "This should not be serialized"
        }
        """;

    private readonly Person _personSubjectForDeserialization = new()
    {
        Name = "Mary",
        Age = 32,
        InternalInfo = null!,
        Address = new Address
        {
            Street = "Elm Street"
        }
    };

    [Fact]
    public void Serializing()
    {
        Assert.Equal(_jsonSubjectForSerialization, JsonSerializer.Serialize(_personSubjectForSerialization, Fixtures.CreateOptions<Person>()));
    }

    [Fact]
    public void Deserializing()
    {
        Assert.Equivalent(_personSubjectForDeserialization, JsonSerializer.Deserialize<Person>(_jsonSubjectForDeserialization, Fixtures.CreateOptions<Person>()));
    }
}
