using Cblx.Blocks.Enums;
using Cblx.Blocks.Extensions;
using Cblx.Blocks.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Cblx.Blocks.Factories;

internal static class HandlerActionDeclarationFactory
{
    public static HandlerActionDeclaration? CreateOrDefault(GeneratorExecutionContext context, MethodDeclarationSyntax methodDeclaration)
    {
        var verb = methodDeclaration.IdentifyHttpVerb();
        if (verb is HttpVerb.Unknown) return default;

        var handlerReturnDeclaration = HandlerReturnDeclarationFactory.Create(context, methodDeclaration);
        var handlerParameterDeclaration = HandlerParameterDeclarationFactory.CreateOrDefault(context, methodDeclaration);

        return new HandlerActionDeclaration(
            methodDeclaration.Identifier.ValueText,
            verb,
            handlerReturnDeclaration,
            handlerParameterDeclaration
        );
    }
}