using System.Text.Json;

namespace Cblx.Blocks.Json.Tests.FluentIgnoreWithRootConfiguration;

public class Tests
{
    private const string _serializingJsonSubject  = """
        {
          "person_name": "Mary",
          "this_street": "Elm Street"
        }
        """;
    private const string _deserializingJsonSubject = """
        {
          "person_name": "Mary",
          "Ignored": "ignore-me",
          "this_street": "Elm Street"
        }
        """;

    private readonly Person _serializingPersonSubject = new ()
    {
        Name = "Mary",
        Address = new AddressVo
        {
            Street = "Elm Street",
            Ignored = "ignore-me"
        }
    };

    private readonly Person _deserializingPersonSubject = new()
    {
        Name = "Mary",
        Address = new AddressVo
        {
            Street = "Elm Street",
            Ignored = null
        }
    };

    [Fact]
    public void Serializing()
    {
        Assert.Equal(_serializingJsonSubject, JsonSerializer.Serialize(_serializingPersonSubject, Fixtures.CreateOptions()));
    }

    [Fact]
    public void Deserializing()
    {
        Assert.Equivalent(_deserializingPersonSubject, JsonSerializer.Deserialize<Person>(_deserializingJsonSubject, Fixtures.CreateOptions()));
    }
}
