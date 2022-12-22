using Microsoft.CodeAnalysis;

namespace Cblx.Blocks.Extensions;

internal static class SymbolExtensions
{
    public static string PascalCaseName(this ISymbol symbol)
        => $"{symbol.Name.Substring(0, 1).ToUpperInvariant()}{symbol.Name.Substring(1)}";

    public static string CamelCaseName(this ISymbol symbol)
        => $"{symbol.Name.Substring(0, 1).ToLowerInvariant()}{symbol.Name.Substring(1)}";


    public static string HandlerName(this ISymbol symbol) => symbol.Name;
}