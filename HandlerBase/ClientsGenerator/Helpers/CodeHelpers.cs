using Cblx.Blocks.Exceptions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

namespace Cblx.Blocks.Helpers;

public static class CodeHelpers
{
    public static string? GetNamespace(GeneratorExecutionContext context, TypeSyntax typeSyntax)
    {
        try
        {
            var semantic = context.Compilation.GetSemanticModel(typeSyntax.SyntaxTree);
            return GetNamespace(typeSyntax, semantic)?.ToString();
        }
        catch
        {
            return null;
        }
    }

    private static INamespaceSymbol? GetNamespace(SyntaxNode node, SemanticModel? semanticModel)
    {
        var name =  semanticModel?.GetTypeInfo(node).Type?.ContainingNamespace;
        return name;
    }

    public static ISymbol? GetDeclaredSymbol(GeneratorExecutionContext context, SyntaxNode syntaxNode)
    {
        try
        {
            var semanticModel = context.Compilation.GetSemanticModel(syntaxNode.SyntaxTree);
            return semanticModel?.GetDeclaredSymbol(syntaxNode, context.CancellationToken);
        }
        catch (Exception)
        {
            return null;
        }
    }
}
