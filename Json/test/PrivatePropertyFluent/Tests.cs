using FluentAssertions;
using System.Text.Json;

namespace Cblx.Blocks.Json.Tests.PrivatePropertyFluent;

public class Tests
{
    private const string _jsonSerializeSubject  = """
        {
          "name": "Mary",
          "person_street": "Elm Street"
        }
        """;

    private const string _jsonDeserializeSubject = """
        {
          "name": "Mary",
          "person_street": "Other Street"
        }
        """;

    private readonly Person _personSubject = new ();
    
    [Fact]
    public void Serializing()
    {
        Assert.Equal(_jsonSerializeSubject, JsonSerializer.Serialize(_personSubject, Fixtures.CreateOptions<Person>()));
    }

    [Fact]
    public void Deserializing()
    {
        var person = JsonSerializer.Deserialize<Person>(_jsonDeserializeSubject, Fixtures.CreateOptions<Person>())!;
        person.Should().BeEquivalentTo(_personSubject);
        person.GetAddress().Street.Should().Be("Other Street");
    }
}
