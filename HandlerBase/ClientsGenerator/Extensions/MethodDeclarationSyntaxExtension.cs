using Cblx.Blocks.Enums;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace Cblx.Blocks.Extensions;

internal static class MethodDeclarationSyntaxExtension
{
    public static HttpVerb IdentifyHttpVerb(this MethodDeclarationSyntax symbol)
    {
        return symbol.Identifier.Text switch
        {
            var content when content.StartsWith("Get") => HttpVerb.Get,
            var content when content.StartsWith("Post") => HttpVerb.Post,
            var content when content.StartsWith("Delete") => HttpVerb.Delete,
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
