namespace Cblx.Blocks;

public class TypedIdTypeConverter<TTypedId> : StringTypeConverterBase<TTypedId> where TTypedId : struct
{
    protected override TTypedId Parse(string s) => (TTypedId)Activator.CreateInstance(typeof(TTypedId), s)!;

    protected override string ToIsoString(TTypedId source) => source.ToString()!;
}