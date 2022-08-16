using Cblx.Blocks.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Cblx.Blocks.Factories;

internal static class HandlerParameterDeclarationFactory
{
    public static HandlerParameterDeclaration? CreateOrDefault(MethodDeclarationSyntax methodDeclarationSyntax)
    {
        var parameter = methodDeclarationSyntax.ParameterList.Parameters.FirstOrDefault();
        if (parameter is null) return default;
        
        var typeName = parameter.Type?.ToFullString();
        if (typeName is null) return default;

        var name = parameter.Identifier.Text.Trim();
        var methodParameterFormat = parameter.ToFullString();

        return new HandlerParameterDeclaration(name, typeName, methodParameterFormat);
    }
}
