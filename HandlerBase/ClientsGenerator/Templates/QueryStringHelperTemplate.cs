namespace Cblx.Blocks.Templates;

internal static class QueryStringHelperTemplate
{
    public const string Source = """
// Auto-generated code
#nullable enable
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Cblx.Blocks;

[ExcludeFromCodeCoverage]
internal static class QueryStringHelper
{
    public static string ToQueryString(object instance)
    {
        var queries = new List<string>();
        var propertiesDictionary = instance.ToPropertyDictionary();

        queries.AddRange(propertiesDictionary.GetQueryStringFromStrings());
        queries.AddRange(propertiesDictionary.GetQueryStringFromNumerics());
        queries.AddRange(propertiesDictionary.GetQueryStringFromEnums());
        queries.AddRange(propertiesDictionary.GetQueryStringFromBool());
        queries.AddRange(propertiesDictionary.GetQueryStringFromDateTime());
        queries.AddRange(propertiesDictionary.GetQueryStringFromIEnumerable());
        queries.AddRange(propertiesDictionary.GetQueryStringFromGuids());
        queries.AddRange(propertiesDictionary.GetQueryStringFromTypedId());

        return queries.JoinQueriesString();
    }

    private static IDictionary<string, object?> ToPropertyDictionary(this object instance)
        => instance
            .GetType()
            .GetProperties()
            .Where(p => p.CanRead)
            .ToDictionary(propertyInfo => propertyInfo.Name, propertyInfo => propertyInfo.GetValue(instance, null));

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
        => CreateQueryString(
            propertiesDictionary,
            o => o.Value is string,
            p => CreateQueryStringArgumentFromPair(p.Key, p.Value as string)
        );

    private static IEnumerable<string> GetQueryStringFromEnums(this IDictionary<string, object?> propertiesDictionary)
        => CreateQueryString(
            propertiesDictionary,
            o => o.Value is Enum,
            p => CreateQueryStringArgumentFromPair(p.Key, p.Value?.ReadEnumNumber())
        );

    private static IEnumerable<string> GetQueryStringFromNumerics(
        this IDictionary<string, object?> propertiesDictionary)
        => CreateQueryString(
            propertiesDictionary,
            o => o.Value is int or long or decimal or float or double,
            p => CreateQueryStringArgumentFromPair(p.Key, p.Value?.ToString())
        );

    private static IEnumerable<string> GetQueryStringFromBool(this IDictionary<string, object?> propertiesDictionary)
        => CreateQueryString(
            propertiesDictionary,
            o => o.Value is bool,
            p => CreateQueryStringArgumentFromPair(p.Key, p.Value?.ToString())
        );

    private static IEnumerable<string> GetQueryStringFromDateTime(
        this IDictionary<string, object?> propertiesDictionary)
        => CreateQueryString(
            propertiesDictionary,
            o => o.Value is DateTime or DateOnly or TimeOnly or TimeSpan or DateTimeOffset,
            p => CreateQueryStringArgumentFromPair(p.Key, p.Value?.ToString())
        );

    private static IEnumerable<string> GetQueryStringFromGuids(this IDictionary<string, object?> propertiesDictionary)
        => CreateQueryString(
            propertiesDictionary,
            o => o.Value is Guid,
            p => CreateQueryStringArgumentFromPair(p.Key, p.Value?.ToString())
        );

    private static IEnumerable<string> GetQueryStringFromTypedId(this IDictionary<string, object?> propertiesDictionary)
        => CreateQueryString(
            propertiesDictionary,
            o => (o.Value?.GetType().Name.EndsWith("Id") ?? false),
            p => CreateQueryStringArgumentFromPair(p.Key, p.Value?.ToString())
        );


    private static IEnumerable<string> GetQueryStringFromIEnumerable(
        this IDictionary<string, object?> propertiesDictionary)
        => CreateQueryString(
            propertiesDictionary,
            o => o.Value is IEnumerable,
            CreateQueryStringFromList
        );

    private static string? ReadEnumNumber(this object value)
    {
        var type = value.GetType();
        return Convert.ChangeType(value, Enum.GetUnderlyingType(type)).ToString();
    }

    private static string CreateQueryStringFromList(KeyValuePair<string, object?> valuePair)
    {
        if (valuePair.Value is string) return string.Empty;
        if (valuePair.Value is not IEnumerable list) return string.Empty;
        var count = 0;
        var dictionary = list.Cast<object?>().ToDictionary(_ => $"{valuePair.Key}[{count++}]", o => o);
        var queries = new List<string>();

        queries.AddRange(dictionary.GetQueryStringFromStrings());
        queries.AddRange(dictionary.GetQueryStringFromNumerics());
        queries.AddRange(dictionary.GetQueryStringFromEnums());
        queries.AddRange(dictionary.GetQueryStringFromDateTime());
        queries.AddRange(dictionary.GetQueryStringFromGuids());
        queries.AddRange(dictionary.GetQueryStringFromTypedId());

        return queries.JoinQueriesString();
    }

    private static string CreateQueryStringArgumentFromPair(string key, string? value)
        => value is null ? string.Empty : $"{Uri.EscapeDataString(key)}={Uri.EscapeDataString(value)}";

    private static string JoinQueriesString(this IEnumerable<string> queries)
        => string.Join("&", queries.Where(q => !string.IsNullOrEmpty(q)).OrderBy(p => p));
}
""";
}
