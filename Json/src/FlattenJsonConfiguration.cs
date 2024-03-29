﻿using System.Linq.Expressions;
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
        GetOwnData(GetPropertyInfo(member)).Attributes.Add(new FlattenJsonIncludePrivatePropertyAttribute());
        return this;
    }

    public FlattenJsonConfiguration<T> HasJsonPropertyName(Expression<Func<T, object?>> member, string jsonPropertyName)
    {
        GetOwnData(GetPropertyInfo(member)).Attributes.Add(new JsonPropertyNameAttribute(jsonPropertyName));
        return this;
    }

    public FlattenJsonConfiguration<T> Ignore(Expression<Func<T, object?>> member)
    {
        GetOwnData(GetPropertyInfo(member)).Attributes.Add(new JsonIgnoreAttribute());
        return this;
    }

    static PropertyInfo GetPropertyInfo(Expression<Func<T, object?>> member) => member.Body switch
    {
        MemberExpression exp => (exp.Member as PropertyInfo)!,
        UnaryExpression unaryExpression => ((unaryExpression.Operand as MemberExpression)?.Member as PropertyInfo)!,
        _ => throw new ArgumentException("Invalid expression", nameof(member))
    };
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

    private readonly Dictionary<PropertyInfo, PropertyData> _ownProperties = new();
    private readonly HashSet<PropertyData> _flattenedProperties = new();

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
            _flattenedProperties.Add(propertyData);
            _ownProperties.Add(property.GetOriginal(), propertyData);
            propertyData.Attributes.AddRange(property.GetCustomAttributes());

            if (property.PropertyType.IsClass && property.HasFluentAttribute())
            {
                propertyData.FluentConfigurationType = property.GetConfigurationType();
                if (property.ShouldFlatten())
                {
                    propertyData.IsFlatten = true;
                    var configuration =
                        propertyData.FluentConfigurationType is null ? new FlattenJsonConfiguration(property.PropertyType) :
                            (Activator.CreateInstance(propertyData.FluentConfigurationType) as FlattenJsonConfiguration)!;
                    foreach (var data in configuration._flattenedProperties)
                    {
                        data.ParentData ??= propertyData;
                        _flattenedProperties.Add(data);
                    }
                }
                else
                {
                    propertyData.IsFlatten = false;
                }
            }
        }
    }

    internal PropertyData? FindDataByJsonPropertyName(string propertyName)
    {
        return _flattenedProperties
            .FirstOrDefault(x => x.GetJsonPropertyName() == propertyName);
    }

    internal PropertyData GetOwnData(PropertyInfo propertyInfo)
    {
        return _ownProperties[propertyInfo.GetOriginal()];
    }

    internal PropertyData[] GetIncludedPropertiesData()
    {
        return _flattenedProperties
        .Where(x => x.ShouldInclude()).ToArray();
    }
}