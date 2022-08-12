using Cblx.Blocks.Enums;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace Cblx.Blocks.Extensions;

internal static class InterfaceDeclarationSyntaxExtension
{
    public static MethodDeclarationSyntax? IdentifyHandlerActionMethod(this InterfaceDeclarationSyntax interfaceDeclaration)
    {
        return interfaceDeclaration
            .Members
            .OfType<MethodDeclarationSyntax>()
            .Where(method => method.IsPublicHandlerAction())
            .Where(p => p.IdentifyHttpVerb() != HttpVerb.Unknown)
            .FirstOrDefault();
    }

    
}
