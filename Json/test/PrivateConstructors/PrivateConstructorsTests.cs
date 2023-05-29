using System.Text.Json;

namespace Cblx.Blocks.Json.Tests.PrivateConstructors;

public class PrivateConstructorsTests
{
    private const string _jsonSubject = """
        {
          "Name": "Mary",
          "Street": "Elm Street"
        }
        """;

    private readonly Person _personSubject = (Person)Activator.CreateInstance(typeof(Person), true)!;

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
