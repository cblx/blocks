using System.Text.Json;

namespace Cblx.Blocks.Json.Tests.Simple;

public class SimpleTests
{
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions()
    {
        WriteIndented = true
    };

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

    public SimpleTests()
    {
        _options.Converters.Add(new FlattenJsonConverter<Person>());
    }

    [Fact]
    public void Serializing()
    {
        Assert.Equal(_jsonSubject, JsonSerializer.Serialize(_personSubject, _options));
    }

    [Fact]
    public void Deserializing()
    {
        Assert.Equivalent(_personSubject, JsonSerializer.Deserialize<Person>(_jsonSubject, _options));
    }
}
