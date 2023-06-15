using System.Reflection;
using System.Text.Json.Serialization;

namespace Cblx.Blocks;

internal class PropertyData
{
    public PropertyData(PropertyInfo propertyInfo)
    {
        PropertyInfo = propertyInfo;
    }
    public bool IsFlatten { get; set; }
    public PropertyInfo PropertyInfo { get; private set; }
    public PropertyData? ParentData { get; set; }
    public List<Attribute> Attributes { get; private set; } = new();
    public string GetJsonPropertyName()
    {
        return Attributes.OfType<JsonPropertyNameAttribute>().LastOrDefault()?.Name
            ?? PropertyInfo.Name;
    }

    internal bool ShouldInclude()
    {
        return
            !Attributes.OfType<JsonIgnoreAttribute>().Any()
            &&
            (
                PropertyInfo.GetMethod?.IsPublic is true
                ||
                Attributes.OfType<FlattenJsonIncludePrivatePropertyAttribute>().Any()
            );
    }
}


internal record PropertyKey
{
    public required PropertyInfo PropertyInfo { get; set; }
    public required PropertyKey? ParentKey { get; set; }
}