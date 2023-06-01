using System.Text.Json;

namespace Cblx.Blocks.Json.Tests.Complex_FluentAttributeAnnotated;

public class Tests
{
    private const string _jsonSubject  = """
        {
          "my_name": "Mary",
          "Age": 32,
          "Street": "Elm Street"
        }
        """;

    private readonly Person _personSubject = ((Person)Activator.CreateInstance(typeof(Person), true)!)
        .WithValues("Mary", 32, "Elm Street");
    
    [Fact]
    public void Serializing()
    {
        Assert.Equal(_jsonSubject, JsonSerializer.Serialize(_personSubject, Fixtures.CreateOptions()));
    }

    [Fact]
    public void Deserializing()
    {
        Assert.Equivalent(_personSubject, JsonSerializer.Deserialize<Person>(_jsonSubject, Fixtures.CreateOptions()));
    }
}
