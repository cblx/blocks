namespace Cblx.Blocks;

public abstract class StringTypeConverterBase<T> : TypeConverter
{
    protected abstract T Parse(string s);
    protected abstract string ToIsoString(T source);
    

    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
    }

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is string str)
        {
            return Parse(str);
        }
        return base.ConvertFrom(context, culture, value);
    }

    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
    {
        return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
    }
    
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        if (destinationType == typeof(string) && value is T typedValue)
        {
            return ToIsoString(typedValue);
        }
        return base.ConvertTo(context, culture, value, destinationType);
    }
    
}