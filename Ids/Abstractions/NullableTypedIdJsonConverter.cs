namespace Cblx.Blocks;

public class NullableTypedIdJsonConverter<TTypedId>: JsonConverter<TTypedId?> where TTypedId : struct 
{
    public override TTypedId? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TryGetGuid(out var guid)) return (TTypedId)Activator.CreateInstance(typeToConvert, guid)!;
        return null;
    }

    public override void Write(Utf8JsonWriter writer, TTypedId? value, JsonSerializerOptions options)
    {
        if (value is null) writer.WriteNullValue();
        else writer.WriteStringValue(value.ToString());
    }
}