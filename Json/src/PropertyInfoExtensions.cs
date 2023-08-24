using System.Reflection;

namespace Cblx.Blocks;

internal static class PropertyInfoExtensions
{
    public static PropertyKey GetKey(this PropertyInfo propertyInfo, PropertyKey? parentKey = null)
        => new () { PropertyInfo = propertyInfo, ParentKey = parentKey };

    public static PropertyInfo GetOriginal(this PropertyInfo propertyInfo)
        => propertyInfo.DeclaringType!.GetProperty(propertyInfo.Name, FlattenJsonConfiguration.PrivateAndPublicPropertiesAccessility)!;

    public static bool HasFluentAttribute(this PropertyInfo propertyInfo)
        => propertyInfo.GetCustomAttribute<FluentJsonPropertyAttribute>() is not null
        || propertyInfo.GetCustomAttribute<FlattenJsonPropertyAttribute>() is not null;

    public static bool ShouldFlatten(this PropertyInfo propertyInfo)
        => propertyInfo.GetCustomAttribute<FlattenJsonPropertyAttribute>() is not null
        || propertyInfo.GetCustomAttribute<FluentJsonPropertyAttribute>()?.Flatten == true;

    public static Type? GetConfigurationType(this PropertyInfo propertyInfo)
        => propertyInfo.GetCustomAttribute<FlattenJsonPropertyAttribute>()?.ConfigurationType
        ?? propertyInfo.GetCustomAttribute<FluentJsonPropertyAttribute>()?.ConfigurationType;
}
