using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Cblx.Blocks;

public abstract class FlattenJsonConfiguration<T> : FlattenJsonConfiguration
     where T : class
{
    protected FlattenJsonConfiguration() : base(typeof(T))
    {
    }

    public FlattenJsonConfiguration<T> IncludePrivateProperty(Expression<Func<T, object?>> member)
    {
        //penInitializePropertyAttributes(propertyName);
        //_propertyMappings[propertyName].Add(new FlattenJsonIncludePrivatePropertyAttribute());
        GetData(GetPropertyInfo(member)).Attributes.Add(new FlattenJsonIncludePrivatePropertyAttribute());
        return this;
    }

    public FlattenJsonConfiguration<T> IncludePrivateProperty(string propertyName)
    {
        //var data = _flattenedProperties.Values
        //    .FirstOrDefault(_flattenedProperties => 
        //    _flattenedProperties.ParentData is null
        //    &&
        //    _flattenedProperties.PropertyInfo.Name == propertyName
        //);
        var propertyInfo = typeof(T).GetProperty(propertyName, PrivateAndPublicPropertiesAccessility)
            ??
            throw new ArgumentException("Property not found", propertyName);

        GetData(propertyInfo).Attributes.Add(new FlattenJsonIncludePrivatePropertyAttribute());
        //penInitializePropertyAttributes(propertyName);
        //_propertyMappings[propertyName].Add(new FlattenJsonIncludePrivatePropertyAttribute());
        //var propertyInfo = typeof(T).GetProperty(propertyName)
        //    ??
        //    throw new ArgumentException("Property not found", propertyName);
        //_flattenedProperties[propertyInfo].Attributes.Add(new FlattenJsonIncludePrivatePropertyAttribute());
        return this;
    }



    public FlattenJsonConfiguration<T> HasJsonPropertyName(Expression<Func<T, object?>> member, string jsonPropertyName)
    {
        //var memberExpression = member.Body switch
        //{
        //    MemberExpression exp => exp,
        //    UnaryExpression unaryExpression => unaryExpression.Operand as MemberExpression,
        //    _ => throw new ArgumentException("Invalid expression", nameof(member))
        //};
        //InitializePropertyAttributes(memberExpression!.Member.Name);
        //_propertyMappings[memberExpression!.Member.Name].Add(new JsonPropertyNameAttribute(jsonPropertyName));
        GetData(GetPropertyInfo(member)).Attributes.Add(new JsonPropertyNameAttribute(jsonPropertyName));
        return this;
    }

    static PropertyInfo GetPropertyInfo(Expression<Func<T, object?>> member) => member.Body switch
    {
        MemberExpression exp => (exp.Member as PropertyInfo)!,
        UnaryExpression unaryExpression => ((unaryExpression.Operand as MemberExpression)?.Member as PropertyInfo)!,
        _ => throw new ArgumentException("Invalid expression", nameof(member))
    };

    //private void InitializePropertyAttributes(string propertyName)
    //{
    //    if(!_propertyMappings.ContainsKey(propertyName))
    //    {
    //        _propertyMappings.Add(propertyName, new List<Attribute>());
    //    }
    //}
}

internal class PropertyData
{
    public PropertyData(PropertyInfo propertyInfo) 
    { 
        PropertyInfo = propertyInfo;
    }
    public bool IsFlatten { get; set; }
    //public bool IsPrivate { get; set; }
    public PropertyInfo PropertyInfo { get; private set; }
    public PropertyData? ParentData { get; set; }
    public List<Attribute> Attributes { get; private set; } = new();
}

public class FlattenJsonConfiguration
{
    private readonly Type _type;
    internal const BindingFlags PrivateAndPublicPropertiesAccessility =
       // Public or non public properties declared in the type or the base type
       BindingFlags.Instance
       | BindingFlags.Public
#pragma warning disable S3011 // This converter supports serialization of private properties
       | BindingFlags.NonPublic;
#pragma warning restore S3011 

    private readonly Dictionary<PropertyInfo, PropertyData> _flattenedProperties = new();

    internal FlattenJsonConfiguration(Type type)
    {
        _type = type;
        InitializeFlattenedProperties(type);
    }

    //protected readonly Dictionary<string, List<Attribute>> _propertyMappings = new();

    private void InitializeFlattenedProperties(Type type)
    {
        var properties = type.GetProperties(PrivateAndPublicPropertiesAccessility)
            //.Where(p => p.GetMethod?.IsPublic is true || p.GetCustomAttribute<FlattenJsonIncludePrivatePropertyAttribute>() is not null)
            .Where(p => p.GetCustomAttribute<JsonIgnoreAttribute>() is null)
            .ToArray();
        foreach (var property in properties)
        {
            var propertyData = new PropertyData(property);
            _flattenedProperties.Add(property, propertyData);
            propertyData.Attributes.AddRange(property.GetCustomAttributes());
            //_flattenedProperties[property].IsPrivate =
            //    property.GetMethod?.IsPublic is not true
            //    &&
            //    property.GetCustomAttribute<FlattenJsonIncludePrivatePropertyAttribute>() is null;

            // If the property is class and has FlattenJsonPropertyAttribute, initialize it's configuration
            if (property.PropertyType.IsClass && property.GetCustomAttribute<FlattenJsonPropertyAttribute>() is not null)
            {
                propertyData.IsFlatten = true;
                var configurationType = property.GetCustomAttribute<FlattenJsonPropertyAttribute>()!.ConfigurationType;
                var configuration =
                    configurationType is null ? new FlattenJsonConfiguration(property.PropertyType) :
                        (Activator.CreateInstance(configurationType) as FlattenJsonConfiguration)!;
                foreach (var (key, value) in configuration._flattenedProperties)
                {
                    value.ParentData ??= propertyData;
                    _flattenedProperties.Add(key, value);
                }
            }
        }
    }

    //public string GetJsonPropertyName(string propertyName)
    //{
    //    // 1. First try to get from the attributes in dictionary.
    //    // 2. Second try to get from the custom attributes annotated in the property
    //    // 3. Fallback to the property name
    //    return (
    //            _propertyMappings.TryGetValue(propertyName, out var attributes) ?
    //                attributes.OfType<JsonPropertyNameAttribute>().LastOrDefault()?.Name
    //                : _type.GetProperty(propertyName)?.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name
    //            ) ?? propertyName;
    //}

    public string GetJsonPropertyName(PropertyInfo propertyInfo)
    {
        return _flattenedProperties[propertyInfo].Attributes.OfType<JsonPropertyNameAttribute>().LastOrDefault()?.Name
            ?? propertyInfo.Name;
    }

    //internal bool ShouldIncludePrivateProperty(PropertyInfo propertyInfo)
    //{
    //    return _flattenedProperties[propertyInfo].Attributes.OfType<FlattenJsonIncludePrivatePropertyAttribute>().Any();
    //}

    internal bool ShouldInclude(PropertyInfo propertyInfo)
    {
        return propertyInfo.GetMethod?.IsPublic is true
            || _flattenedProperties[propertyInfo].Attributes.OfType<FlattenJsonIncludePrivatePropertyAttribute>().Any();
    }

    internal PropertyData? FindDataByJsonPropertyName(string propertyName)
    {
        return _flattenedProperties.Values.FirstOrDefault(x => GetJsonPropertyName(x.PropertyInfo) == propertyName);
    }

    internal PropertyData GetData(PropertyInfo propertyInfo)
    {
        return _flattenedProperties[propertyInfo];
    }

    internal PropertyData[] GetIncludedPropertiesData()
    {
        return _flattenedProperties.Values.Where(x => ShouldInclude(x.PropertyInfo)).ToArray();
    }

    //internal bool ShouldIncludePrivateProperty(string name)
    //{
    //    return _propertyMappings.TryGetValue(name, out var attributes)
    //        && attributes.OfType<FlattenJsonIncludePrivatePropertyAttribute>().Any();
    //}
}