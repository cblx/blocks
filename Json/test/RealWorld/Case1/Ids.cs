using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Cblx.Blocks.Json.Tests.RealWorld.Case1;

[TypeConverter(typeof(IdTypeConverter<TbId>))]
[JsonConverter(typeof(IdConverterFactory))]
public partial record TbId(Guid Guid) : Id(Guid)
{
    public TbId(string guidString) : this(new Guid(guidString)) { }
    public static implicit operator Guid(TbId? id) => id?.Guid ?? Guid.Empty;
    public static implicit operator Guid?(TbId? id) => id?.Guid;
    public static explicit operator TbId(Guid guid) => new TbId(guid);
    public static TbId Empty { get; } = new TbId(Guid.Empty);
    public static TbId NewId() => new TbId(Guid.NewGuid());
    public override string ToString() => Guid.ToString();
}

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
public abstract record Id(Guid Guid);