using System.Text.Json.Serialization;
using System.Text.Json;

namespace Cblx.Blocks;

public class FlattenJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) => true;
    
    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        => (Activator.CreateInstance(typeof(FlattenJsonConverter<>).MakeGenericType(typeToConvert)) as JsonConverter)!;
}
