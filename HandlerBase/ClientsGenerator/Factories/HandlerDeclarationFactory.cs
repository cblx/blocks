using Cblx.Blocks.Extensions;
using Cblx.Blocks.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Cblx.Blocks.Factories;

internal static class HandlerDeclarationFactory
{
    public static HandlerDeclaration? CreateOrDefault(InterfaceDeclarationSyntax interfaceDeclaration)
    {
        var handlerActionMethod = interfaceDeclaration.IdentifyHandlerActionMethod();
        if (handlerActionMethod is null) return default;

        var name = interfaceDeclaration.Identifier.Text;


        return new HandlerDeclaration(name);
    }

    
}
