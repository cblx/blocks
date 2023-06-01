using System.ComponentModel;
using System.Globalization;

namespace Cblx.Blocks.Json.Tests.RealWorld.Case1;

public class IdTypeConverter<TId> : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    {
        if (sourceType == typeof(string))
        {
            return true;
        }

        return base.CanConvertFrom(context, sourceType);
    }

    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
        return Activator.CreateInstance(typeof(TId), value);
    }
}