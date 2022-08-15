using Cblx.Blocks.Factories;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Cblx.Blocks.Helpers;

internal static class AnalyzeNodeHelper
{
    public static void Analyze(ReturnDeclaration declaration, SyntaxNode? node)
    {
        switch (node)
        {
            case IdentifierNameSyntax syntax: declaration.ProcessIdentifierNameSyntax(syntax); break;
            case GenericNameSyntax syntax: declaration.ProcessGenericNameSyntax(syntax); break;
            case ArrayTypeSyntax syntax: declaration.ProcessArrayTypeSyntax(syntax); break;
        }
    }

    private static void ProcessIdentifierNameSyntax(this ReturnDeclaration declaration, IdentifierNameSyntax syntax)
    {

        switch (syntax.Identifier.Text)
        {
            case var typeName when
                 typeName.StartsWith("Task") ||
                 typeName.StartsWith("ValueTask"):
                declaration.ProcessTaskOrValueTaskType(syntax); return;
        }

        declaration.TypeName = syntax.Identifier.Text.Trim();
        declaration.HasVoid = false;

        if (!declaration.ManipulationFormat.Contains(declaration.TypeName))
        {
            declaration.ManipulationFormat = declaration.TypeName;
        }
    }

    private static void ProcessArrayTypeSyntax(this ReturnDeclaration declaration, ArrayTypeSyntax syntax)
    {
        declaration.TypeName = syntax.ToFullString().Trim();

        if (!declaration.ManipulationFormat.Contains(declaration.TypeName))
        {
            declaration.ManipulationFormat = declaration.TypeName;
        }
    }

    private static void ProcessGenericNameSyntax(this ReturnDeclaration declaration, GenericNameSyntax syntax)
    {
        switch (syntax.Identifier.Text)
        {
            case var typeName when
                 typeName.StartsWith("Task") ||
                 typeName.StartsWith("ValueTask"):
                declaration.ProcessTaskOrValueTaskType(syntax); break;

            case var typeName when typeName.StartsWith("IEnumerable"):
                declaration.ProcessIEnumerableType(syntax); break;
        }
    }

    private static void ProcessTaskOrValueTaskType(this ReturnDeclaration declaration, SyntaxNode syntax)
    {
        declaration.HasAsync = true;
        declaration.HasVoid = true;
        declaration.TypeName = syntax.ToFullString();
    }

    private static void ProcessIEnumerableType(this ReturnDeclaration declaration, SyntaxNode syntax)
    {
        if (!declaration.ManipulationFormat.Contains(syntax.ToFullString()))
        {
            declaration.ManipulationFormat = syntax.ToFullString();
            declaration.HasVoid = false;
        }
    }
}
