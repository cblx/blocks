using System.Text.Json;

namespace Cblx.Blocks.Json.Tests.AttributeUsageAndNullValues;

public class Tests
{
    private const string _jsonSubject  = """
        {
          "Name": null,
          "Age": 0,
          "Description": null,
          "Street": null,
          "Number": 0,
          "District": null
        }
        """;

    private readonly Person _personSubject = new ();

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
