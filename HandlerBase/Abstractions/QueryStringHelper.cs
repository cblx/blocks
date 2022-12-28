using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Cblx.Blocks;

[ExcludeFromCodeCoverage]
public static class QueryStringHelper
{
    public static string ToQueryString(object instance)
    {
        var queries = new List<string>();
        var propertiesDictionary = instance.ToPropertyDictionary();

        queries.AddRange(propertiesDictionary.GetQueryStringFromStrings());
        queries.AddRange(propertiesDictionary.GetQueryStringFromIEnumerable());
        queries.AddRange(propertiesDictionary.GetQueryStringFromOtherType());

        return queries.JoinQueriesString();
    }

    private static IDictionary<string, object?> ToPropertyDictionary(this object instance)
    {
        return instance
            .GetType()
            .GetProperties()
            .Where(p => p.CanRead)
            .ToDictionary(propertyInfo => propertyInfo.Name, propertyInfo => propertyInfo.GetValue(instance, null));
    }

    private static IEnumerable<string> CreateQueryString(
        IDictionary<string, object?> propertiesDictionary,
        Func<KeyValuePair<string, object?>, bool> filter,
        Func<KeyValuePair<string, object?>, string> buildQueryString)
    {
        return propertiesDictionary
            .Where(filter)
            .Select(buildQueryString)
            .ToList();
    }

    private static IEnumerable<string> GetQueryStringFromStrings(this IDictionary<string, object?> propertiesDictionary)
    {
        return CreateQueryString(
            propertiesDictionary,
            o => o.Value is string,
            p => CreateQueryStringArgumentFromPair(p.Key, p.Value as string)
        );
    }
    
    private static IEnumerable<string> GetQueryStringFromIEnumerable(
        this IDictionary<string, object?> propertiesDictionary)
    {
        return CreateQueryString(
            propertiesDictionary,
            o => o.Value is IEnumerable,
            CreateQueryStringFromList
        );
    }
    
    private static string CreateQueryStringFromList(KeyValuePair<string, object?> valuePair)
    {
        if (valuePair.Value is string) return string.Empty;
        if (valuePair.Value is not IEnumerable list) return string.Empty;
        var count = 0;
        var dictionary = list.Cast<object?>().ToDictionary(_ => $"{valuePair.Key}[{count++}]", o => o);
        var queries = new List<string>();

        queries.AddRange(dictionary.GetQueryStringFromStrings());
        queries.AddRange(dictionary.GetQueryStringFromOtherType());

        return queries.JoinQueriesString();
    }

    private static IEnumerable<string> GetQueryStringFromOtherType(this IDictionary<string, object?> propertiesDictionary)
    {
        return CreateQueryString(
            propertiesDictionary,
            o => o.Value is not string and not IEnumerable,
            p => CreateQueryStringArgumentFromPairWithValueJsonFormat(p.Key, JsonSerializer.Serialize(p.Value))
        );
    }

    private static string CreateQueryStringArgumentFromPairWithValueJsonFormat(string key, string? value)
    {
        return value is null or "null" ? 
            string.Empty : 
            $"{Uri.EscapeDataString(key)}={RemoveQuotationMarksAndFormatEscapeDataString(value)}";
    }

    private static string RemoveQuotationMarksAndFormatEscapeDataString(string value)
    {
        return value[0] is '"' ? Uri.EscapeDataString(value[1..^1]) : Uri.EscapeDataString(value);
    }

    private static string CreateQueryStringArgumentFromPair(string key, string? value)
        => value is null ? string.Empty : $"{Uri.EscapeDataString(key)}={Uri.EscapeDataString(value)}";

    private static string JoinQueriesString(this IEnumerable<string> queries)
        => string.Join("&", queries.Where(q => !string.IsNullOrEmpty(q)).OrderBy(p => p));
}

