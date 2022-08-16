using System.Collections.Generic;
using Cblx.Blocks.Helpers;
using Cblx.Blocks.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Cblx.Blocks.Factories;

internal static class HandlerReturnDeclarationFactory
{
    public static HandlerReturnDeclaration Create(MethodDeclarationSyntax methodDeclarationSyntax)
    {
        var returnMethodTree = methodDeclarationSyntax.ReturnType.DescendantNodesAndTokensAndSelf();
        var returnDeclaration = IdentifyAndProcessMethodReturnTree(returnMethodTree);

        return new HandlerReturnDeclaration(
            returnDeclaration.TypeName,
            methodDeclarationSyntax.ReturnType.ToFullString(),
            returnDeclaration.ManipulationFormat,
            returnDeclaration.HasVoid,
            returnDeclaration.HasAsync
        );
    }

    private static ReturnDeclarationDto IdentifyAndProcessMethodReturnTree(IEnumerable<SyntaxNodeOrToken> tree)
    {
        var returnDeclaration = new ReturnDeclarationDto();

        foreach (var nodeOrToken in tree)
        {
            if(nodeOrToken.IsNode)
                AnalyzeNodeHelper.Analyze(returnDeclaration, nodeOrToken.AsNode());

            //if (nodeOrToken.IsToken)
            //    AnalyzeTokenHelper.Analyze(returnDeclaration, nodeOrToken.AsToken());
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
}
