using System.Reflection;

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
}
