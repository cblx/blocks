using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Cblx.Blocks.Helpers;

internal static class AttributeHelper
{
    public static bool ContainsGenerateClientAttribute(InterfaceDeclarationSyntax interfaceDeclaration)
        => CreateQueryInternal(interfaceDeclaration).Any();

    private static IEnumerable<AttributeSyntax> CreateQueryInternal(InterfaceDeclarationSyntax interfaceDeclaration)
        => interfaceDeclaration
            .AttributeLists
            .SelectMany(attrList => attrList.Attributes)
            .Where(attr => attr.ToFullString().Contains("GenerateClient"))
            .OfType<AttributeSyntax>();
    
}
