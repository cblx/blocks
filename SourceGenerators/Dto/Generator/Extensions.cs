using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace Cblx.Blocks.SourceGenerators.Dto;
internal static class Extensions
{
    public static IEnumerable<IPropertySymbol> GetAllProperties(this ITypeSymbol type)
    {
        return type.GetBaseTypesAndThis().SelectMany(t => t.GetMembers())
                                          .Where(m => m.Kind == SymbolKind.Property)
                                          .Cast<IPropertySymbol>();
    }

    public static IEnumerable<ITypeSymbol> GetBaseTypesAndThis(this ITypeSymbol type)
    {
        var current = type;
        while (current != null)
        {
            yield return current;
            current = current.BaseType;
        }
    }
}
