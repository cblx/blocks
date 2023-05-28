using System.Net.Http.Headers;
using System.Text.Json;

namespace Cblx.Blocks.Json.Tests;

public static class Fixtures
{
    public static JsonSerializerOptions CreateOptions<T>()
    {
        var converter = new FlattenJsonConverter<T>();
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        options.Converters.Add(converter);
        return options;
    }
}
