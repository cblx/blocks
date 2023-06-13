using System.Reflection;

namespace Cblx.Blocks;

internal static class PropertyInfoExtensions
{
    public static PropertyInfo GetOriginal(this PropertyInfo propertyInfo)
        => propertyInfo.DeclaringType!.GetProperty(propertyInfo.Name, FlattenJsonConfiguration.PrivateAndPublicPropertiesAccessility)!;
}
