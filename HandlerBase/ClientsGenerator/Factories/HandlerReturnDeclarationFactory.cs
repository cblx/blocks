using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cblx.Blocks.Helpers;
using Cblx.Blocks.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Cblx.Blocks.Factories;

internal static class HandlerReturnDeclarationFactory
{
    public static HandlerReturnDeclaration Create(GeneratorExecutionContext context,
        MethodDeclarationSyntax methodDeclarationSyntax)
    {
        var returnMethodTree = methodDeclarationSyntax.ReturnType.DescendantNodesAndTokensAndSelf();
        var returnDeclaration = IdentifyAndProcessMethodReturnTree(context, returnMethodTree);

        return new HandlerReturnDeclaration(
            methodDeclarationSyntax.ReturnType.ToFullString().TrimStart().TrimEnd(),
            returnDeclaration.ManipulationTypeBuilder(),
            returnDeclaration.HasVoid,
            returnDeclaration.HasAsync,
            returnDeclaration.Uses
        );
    }

    private static ReturnDeclarationDto IdentifyAndProcessMethodReturnTree(GeneratorExecutionContext context,
        IEnumerable<SyntaxNodeOrToken> tree)
    {
        var returnDeclaration = new ReturnDeclarationDto();

        tree = tree.ToList();
        
        tree.Where(t => t.IsNode)
            .Select(t => t.AsNode())
            .ToList()
            .ForEach(node => returnDeclaration.AnalyzeAndAddUsing(context, node));
        
        tree.Where(t => t.IsToken)
            .Select(t => t.AsToken())
            .ToList()
            .ForEach(node => returnDeclaration.AppendToken(node));

        return returnDeclaration;
    }
}

internal record ReturnDeclarationDto
{
    public StringBuilder ManipulationTypeStringBuilder { get; } = new ();
    public bool HasVoid { get; set; }
    public bool HasAsync { get; set; }
    public List<string> Uses { get; } = new();
}