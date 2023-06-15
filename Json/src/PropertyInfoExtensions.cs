using System.Reflection;

namespace Cblx.Blocks;

internal static class PropertyInfoExtensions
{
    public static PropertyKey GetKey(this PropertyInfo propertyInfo, PropertyKey? parentKey = null)
        => new () { PropertyInfo = propertyInfo, ParentKey = parentKey };

    public static PropertyInfo GetOriginal(this PropertyInfo propertyInfo)
        => propertyInfo.DeclaringType!.GetProperty(propertyInfo.Name, FlattenJsonConfiguration.PrivateAndPublicPropertiesAccessility)!;
}
