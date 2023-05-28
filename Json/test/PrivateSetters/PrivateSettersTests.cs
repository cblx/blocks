using System.Text.Json;

namespace Cblx.Blocks.Json.Tests.PrivateSetters;

public class PrivateSettersTests
{
    private const string _jsonSubject  = """
        {
          "name": "Mary",
          "person_street": "Elm Street"
        }
        """;

    private readonly Person _personSubject = new ();
    
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
