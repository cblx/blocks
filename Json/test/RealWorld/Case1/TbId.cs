using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Cblx.Blocks.Json.Tests.RealWorld.Case1;

[TypeConverter(typeof(IdTypeConverter<TbId>))]
[JsonConverter(typeof(IdConverterFactory))]
public partial record TbId(Guid Guid) : Id(Guid)
{
    public TbId(string guidString) : this(new Guid(guidString)) { }
    public static implicit operator Guid(TbId? id) => id?.Guid ?? Guid.Empty;
    public static implicit operator Guid?(TbId? id) => id?.Guid;
    public static explicit operator TbId(Guid guid) => new (guid);
    public static TbId Empty { get; } = new TbId(Guid.Empty);
    public static TbId NewId() => new (Guid.NewGuid());
    public override string ToString() => Guid.ToString();
}
