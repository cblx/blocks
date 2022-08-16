using Cblx.Blocks.Enums;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace Cblx.Blocks.Extensions;

internal static class MethodDeclarationSyntaxExtension
{
    public static HttpVerb IdentifyHttpVerb(this MethodDeclarationSyntax symbol)
    {
        var possibleVerb = symbol.Identifier.Text.Trim();
        
        return possibleVerb switch
        {
            _ when possibleVerb.StartsWith("Get") => HttpVerb.Get,
            _ when possibleVerb.StartsWith("Post") => HttpVerb.Post,
            _ when possibleVerb.StartsWith("Delete") => HttpVerb.Delete,
            _ => HttpVerb.Unknown
        };
    }

    public static bool IsPublicHandlerAction(this MethodDeclarationSyntax methodDeclaration)
    {
        return methodDeclaration
            .Modifiers
            .Any(token => token.Text.Trim() is "public");
    }
}
