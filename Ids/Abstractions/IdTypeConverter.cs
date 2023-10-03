namespace Cblx.Blocks;

public class IdTypeConverter<TId> : TypeConverter where TId : struct
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
    }

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        return Activator.CreateInstance(typeof(TId), value);
    }
}