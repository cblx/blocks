namespace Cblx.Blocks;

public class IdConverterFactory<TId>: JsonConverterFactory where TId : struct
{
    public override bool CanConvert(Type typeToConvert)
    {
        var canConvert = typeToConvert == typeof(TId);
        return canConvert;
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return (Activator.CreateInstance(typeof(IdConverterGeneric<>).MakeGenericType(typeToConvert)) as JsonConverter)!;
    }
}