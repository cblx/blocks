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
        //GetData(GetPropertyName(member)).Attributes.Add(new FlattenJsonIncludePrivatePropertyAttribute());
        GetData(GetPropertyInfo(member)).Attributes.Add(new FlattenJsonIncludePrivatePropertyAttribute());
        return this;
    }

    //public FlattenJsonConfiguration<T> IncludePrivateProperty(string propertyName)
    //{
    //    GetData(propertyName).Attributes.Add(new FlattenJsonIncludePrivatePropertyAttribute());
    //    return this;
    //}

    public FlattenJsonConfiguration<T> HasJsonPropertyName(Expression<Func<T, object?>> member, string jsonPropertyName)
    {
        //GetData(GetPropertyName(member)).Attributes.Add(new JsonPropertyNameAttribute(jsonPropertyName));
        GetData(GetPropertyInfo(member)).Attributes.Add(new JsonPropertyNameAttribute(jsonPropertyName));
        return this;
    }

    public FlattenJsonConfiguration<T> Ignore(Expression<Func<T, object?>> member)
    {
        //GetData(GetPropertyName(member)).Attributes.Add(new JsonIgnoreAttribute());
        GetData(GetPropertyInfo(member)).Attributes.Add(new JsonIgnoreAttribute());
        return this;
    }

    static PropertyInfo GetPropertyInfo(Expression<Func<T, object?>> member) => member.Body switch
    {
        MemberExpression exp => (exp.Member as PropertyInfo)!,
        UnaryExpression unaryExpression => ((unaryExpression.Operand as MemberExpression)?.Member as PropertyInfo)!,
        _ => throw new ArgumentException("Invalid expression", nameof(member))
    };

    //static string GetPropertyName(Expression<Func<T, object?>> member) => GetPropertyInfo(member).Name;
}

public class FlattenJsonConfiguration
{
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
        InitializeFlattenedProperties(type);
    }

    private void InitializeFlattenedProperties(Type type)
    {
        var properties = type.GetProperties(PrivateAndPublicPropertiesAccessility)
            .Where(p => p.GetCustomAttribute<JsonIgnoreAttribute>() is null)
            .ToArray();
        foreach (var property in properties)
        {
            var propertyData = new PropertyData(property);
            //_flattenedProperties.Add(property.Name, propertyData);
            _flattenedProperties.Add(property.GetOriginal(), propertyData);
            propertyData.Attributes.AddRange(property.GetCustomAttributes());

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

    public string GetJsonPropertyName(PropertyInfo propertyInfo)
    {
        //return _flattenedProperties[propertyInfo.Name].Attributes.OfType<JsonPropertyNameAttribute>().LastOrDefault()?.Name
        return _flattenedProperties[propertyInfo.GetOriginal()].Attributes.OfType<JsonPropertyNameAttribute>().LastOrDefault()?.Name
            ?? propertyInfo.Name;
    }

    internal bool ShouldInclude(PropertyInfo propertyInfo)
    {
        return
            //!_flattenedProperties[propertyInfo.Name].Attributes.OfType<JsonIgnoreAttribute>().Any()
            !_flattenedProperties[propertyInfo.GetOriginal()].Attributes.OfType<JsonIgnoreAttribute>().Any()
            &&
            (
                propertyInfo.GetMethod?.IsPublic is true
                ||
                //_flattenedProperties[propertyInfo.Name].Attributes.OfType<FlattenJsonIncludePrivatePropertyAttribute>().Any()
                _flattenedProperties[propertyInfo.GetOriginal()].Attributes.OfType<FlattenJsonIncludePrivatePropertyAttribute>().Any()
            );
    }

    internal PropertyData? FindDataByJsonPropertyName(string propertyName)
    {
        return _flattenedProperties
            .Values
            .FirstOrDefault(x => GetJsonPropertyName(x.PropertyInfo) == propertyName);
    }

    internal PropertyData GetData(PropertyInfo propertyInfo)
    {
        return _flattenedProperties[propertyInfo.GetOriginal()];
    }

    //internal PropertyData GetData(string propertyName)
    //{
    //    return _flattenedProperties[propertyName];
    //}

    internal PropertyData[] GetIncludedPropertiesData()
    {
        return _flattenedProperties.Values.Where(x => ShouldInclude(x.PropertyInfo)).ToArray();
    }
}