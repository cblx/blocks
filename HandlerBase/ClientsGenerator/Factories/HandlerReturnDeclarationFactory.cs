using System.Collections.Generic;
using Cblx.Blocks.Helpers;
using Cblx.Blocks.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Cblx.Blocks.Factories;

internal static class HandlerReturnDeclarationFactory
{
    public static HandlerReturnDeclaration Create(GeneratorExecutionContext context, MethodDeclarationSyntax methodDeclarationSyntax)
    {
        var returnMethodTree = methodDeclarationSyntax.ReturnType.DescendantNodesAndTokensAndSelf();
        var returnDeclaration = IdentifyAndProcessMethodReturnTree(context, returnMethodTree);

        return new HandlerReturnDeclaration(
            returnDeclaration.TypeName,
            methodDeclarationSyntax.ReturnType.ToFullString().TrimStart().TrimEnd(),
            returnDeclaration.ManipulationFormat.TrimStart().TrimEnd(),
            returnDeclaration.HasVoid,
            returnDeclaration.HasAsync,
            returnDeclaration.Namespace
        );
    }

    private static ReturnDeclarationDto IdentifyAndProcessMethodReturnTree(GeneratorExecutionContext context, IEnumerable<SyntaxNodeOrToken> tree)
    {
        var returnDeclaration = new ReturnDeclarationDto();

        foreach (var nodeOrToken in tree)
        {
            if (nodeOrToken.IsNode)
                AnalyzeNodeHelper.Analyze(context,returnDeclaration, nodeOrToken.AsNode());
        }

        return returnDeclaration;
    }
}

internal record ReturnDeclarationDto
{
    public string TypeName { get; set; } = string.Empty;
    public string ManipulationFormat { get; set; } = string.Empty;
    public bool HasVoid { get; set; }
    public bool HasAsync { get; set; }
    public string Namespace { get; set; } = string.Empty;
}