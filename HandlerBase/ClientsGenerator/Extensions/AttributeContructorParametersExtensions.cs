using Cblx.Blocks.Exceptions;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Cblx.Blocks.Extensions;

internal static class AttributeConstructorParametersExtensions
{
    public static T GetOrThrow<T>(this in ImmutableArray<TypedConstant> attributeParameters, int index, string propertyName)
       => (T)(attributeParameters[index].Value ?? throw new ConfigurationException($"{propertyName} cannot be null."));

    public static string GetOrThrow(this in ImmutableArray<TypedConstant> attributeParameters, int index, string propertyName)
        => attributeParameters.GetOrThrow<string>(index, propertyName);

    public static T GetOrThrow<T>(this in ImmutableArray<KeyValuePair<string, TypedConstant>> attributeParameters, string propertyName)
    {
        var element = attributeParameters.FirstOrDefault(p => p.Key == propertyName).Value;
        return (T)(element.Value ?? throw new ConfigurationException($"{propertyName} cannot be null."));

    }

    public static string GetOrThrow(this in ImmutableArray<KeyValuePair<string, TypedConstant>> attributeParameters, string propertyName)
    {
        return attributeParameters.GetOrThrow<string>(propertyName);
    }

    public static T? GetOrDefault<T>(this in ImmutableArray<KeyValuePair<string, TypedConstant>> attributeParameters, string propertyName)
    {
        if (attributeParameters.Length is 0) return default;
        if (attributeParameters.Any(p => p.Key != propertyName)) return default;

        var element = attributeParameters.FirstOrDefault(p => p.Key == propertyName).Value;

        return (T?)(element.Value);
    }
}
