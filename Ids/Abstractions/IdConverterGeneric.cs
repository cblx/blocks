namespace Cblx.Blocks;

public class IdConverterGeneric<TId> : JsonConverter<TId> where TId : struct
{
    public override bool CanConvert(Type typeToConvert)
    {
        var canConvert = typeToConvert == typeof(TId);
        return canConvert;
    }

    public override TId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        object? id = default!;
        
        if (reader.TryGetGuid(out var guid))
        {
            id = Activator.CreateInstance(typeToConvert, guid);
        }
        
        return (TId)id!;
    }

    public override void Write(Utf8JsonWriter writer, TId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}