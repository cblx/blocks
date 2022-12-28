using Cblx.Blocks.Helpers;
using Cblx.Blocks.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Cblx.Blocks.Factories;

internal static class HandlerParameterDeclarationFactory
{
    public static HandlerParameterDeclaration? CreateOrDefault(MethodDeclarationSyntax methodDeclarationSyntax)
    {
        var parameter = methodDeclarationSyntax.ParameterList.Parameters.FirstOrDefault();

        var typeName = parameter?.Type?.ToFullString();
        if (typeName is null) return default;

        var name = parameter!.Identifier.Text.Trim();
        var methodParameterFormat = parameter.ToFullString();

        var parameterNamespace = CodeHelpers.GetNamespace(parameter.Type!)!;

        return new HandlerParameterDeclaration(name, typeName, methodParameterFormat, parameterNamespace);
    }


}