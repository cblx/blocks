using System.Text.Json;

namespace Cblx.Blocks.Json.Tests.Annotated;

public class AnnotatedTests
{
    private const string _jsonSubject  = """
        {
          "name": "Mary",
          "person_street": "Elm Street"
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
        Assert.Equal(_jsonSubject, JsonSerializer.Serialize(_personSubject, Fixtures.CreateOptions<Person>()));
    }

    [Fact]
    public void Deserializing()
    {
        Assert.Equivalent(_personSubject, JsonSerializer.Deserialize<Person>(_jsonSubject, Fixtures.CreateOptions<Person>()));
    }
}
