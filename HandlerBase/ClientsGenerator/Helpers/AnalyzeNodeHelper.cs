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
        declaration.TypeName = syntax.Identifier.Text.Trim();

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

    private static void ProcessTaskOrValueTaskType(this ReturnDeclaration declaration, GenericNameSyntax syntax)
    {
        declaration.HasAsync = true;
        declaration.TypeName = syntax.Identifier.Text;
    }

    private static void ProcessIEnumerableType(this ReturnDeclaration declaration, GenericNameSyntax syntax)
    {
        if (!declaration.ManipulationFormat.Contains(syntax.Identifier.Text))
        {
            declaration.ManipulationFormat = syntax.ToFullString();
        }
    }
}
