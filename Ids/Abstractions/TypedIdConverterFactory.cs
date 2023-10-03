namespace Cblx.Blocks;

public class TypedIdConverterFactory<TTypedId>: JsonConverterFactory where TTypedId : struct
{
    public override bool CanConvert(Type typeToConvert)
    {
        var canConvert = typeToConvert == typeof(TTypedId) || typeToConvert == typeof(TTypedId?);
        return canConvert;
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        if (typeToConvert == typeof(TTypedId))
        {
            return new TypedIdJsonConverter<TTypedId>();
        }

        return new NullableTypedIdJsonConverter<TTypedId>();
    }
}