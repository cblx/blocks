using System.Text.Json.Serialization;
using System.Text.Json;

namespace Cblx.Blocks.Json.Tests.RealWorld.Case1;

public class IdConverterGeneric<TId> : JsonConverter<TId>
    where TId : Id
{
    public override bool CanConvert(Type typeToConvert)
    {
        bool canConvert = typeToConvert.BaseType == typeof(Id);
        return canConvert;
    }

    public override TId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        object id = null;
        if (reader.TryGetGuid(out var guid))
        {
            id = Activator.CreateInstance(typeToConvert, new object[] { guid });
        }
        return (TId)id;
    }

    public override void Write(Utf8JsonWriter writer, TId value, JsonSerializerOptions options) => writer.WriteStringValue(value.Guid.ToString());
}
