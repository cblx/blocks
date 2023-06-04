using System.Text.Json.Serialization;
using System.Text.Json;

namespace Cblx.Blocks.Json.Tests.RealWorld.Case1;

public class IdConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        bool canConvert = typeToConvert.BaseType == typeof(Id);
        return canConvert;
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        => (Activator.CreateInstance(typeof(IdConverterGeneric<>).MakeGenericType(typeToConvert)) as JsonConverter)!;
}
