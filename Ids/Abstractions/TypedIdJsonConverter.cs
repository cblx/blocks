namespace Cblx.Blocks;

public class TypedIdJsonConverter<TTypedId>: JsonConverter<TTypedId> where TTypedId : struct 
{
    public override TTypedId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TryGetGuid(out var guid)) { return (TTypedId)Activator.CreateInstance(typeToConvert, guid)!; }
        return default;
    }

    public override void Write(Utf8JsonWriter writer, TTypedId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}