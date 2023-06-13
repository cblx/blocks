using System.Text.Json;

namespace Cblx.Blocks.Json.Tests.FluentIgnore;

public class Tests
{
    private const string _serializingJsonSubject  = """
        {
          "Name": "Mary",
          "this_street": "Elm Street"
        }
        """;
    private const string _deserializingJsonSubject = """
        {
          "Name": "Mary",
          "Ignored": "ignore-me",
          "this_street": "Elm Street"
        }
        """;

    private readonly Person _serializingPersonSubject = new ()
    {
        Name = "Mary",
        Address = new Address
        {
            Street = "Elm Street",
            Ignored = "ignore-me"
        }
    };

    private readonly Person _deserializingPersonSubject = new()
    {
        Name = "Mary",
        Address = new Address
        {
            Street = "Elm Street",
            Ignored = null
        }
    };

    [Fact]
    public void Serializing()
    {
        Assert.Equal(_serializingJsonSubject, JsonSerializer.Serialize(_serializingPersonSubject, Fixtures.CreateOptions<Person>()));
    }

    [Fact]
    public void Deserializing()
    {
        Assert.Equivalent(_deserializingPersonSubject, JsonSerializer.Deserialize<Person>(_deserializingJsonSubject, Fixtures.CreateOptions<Person>()));
    }
}
