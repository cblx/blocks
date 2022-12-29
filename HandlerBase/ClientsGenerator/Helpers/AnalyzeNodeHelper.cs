using Cblx.Blocks.Factories;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Cblx.Blocks.Helpers;

internal static class AnalyzeNodeHelper
{
    public static void Analyze(GeneratorExecutionContext context, ReturnDeclarationDto declarationDto, SyntaxNode? node)
    {
        switch (node)
        {
            case IdentifierNameSyntax syntax:
                declarationDto.ProcessIdentifierNameSyntax(syntax, context);
                break;
            case GenericNameSyntax syntax:
                declarationDto.ProcessGenericNameSyntax(syntax);
                break;
            case ArrayTypeSyntax syntax:
                declarationDto.ProcessArrayTypeSyntax(syntax);
                break;
        }
    }

    private static void ProcessIdentifierNameSyntax(
        this ReturnDeclarationDto declarationDto,
        IdentifierNameSyntax syntax,
        GeneratorExecutionContext context)
    {
        var name = syntax.Identifier.Text.Trim();

        switch (name)
        {
            case var _ when name.StartsWith("Task") || name.StartsWith("ValueTask"):
                declarationDto.ProcessTaskOrValueTaskType(syntax);
                return;
        }

        declarationDto.Namespace = CodeHelpers.GetNamespace(context,syntax)!;
        declarationDto.TypeName = name;
        declarationDto.HasVoid = false;

        if (!declarationDto.ManipulationFormat.Contains(name))
        {
            declarationDto.ManipulationFormat = name;
        }
    }

    private static void ProcessArrayTypeSyntax(this ReturnDeclarationDto declarationDto, ArrayTypeSyntax syntax)
    {
        declarationDto.TypeName = syntax.ToFullString().Trim();

        if (!declarationDto.ManipulationFormat.Contains(declarationDto.TypeName))
        {
            declarationDto.ManipulationFormat = declarationDto.TypeName;
        }
    }

    private static void ProcessGenericNameSyntax(this ReturnDeclarationDto declarationDto, GenericNameSyntax syntax)
    {
        var genericName = syntax.Identifier.Text.Trim();

        switch (genericName)
        {
            case var _ when genericName.StartsWith("Task") || genericName.StartsWith("ValueTask"):
                declarationDto.ProcessTaskOrValueTaskType(syntax);
                break;

            case var _ when genericName.StartsWith("IEnumerable"):
                declarationDto.ProcessIEnumerableType(syntax);
                break;
        }
    }

    private static void ProcessTaskOrValueTaskType(this ReturnDeclarationDto declarationDto, SyntaxNode syntax)
    {
        declarationDto.HasAsync = true;
        declarationDto.HasVoid = true;
        declarationDto.TypeName = syntax.ToFullString();
    }

    private static void ProcessIEnumerableType(this ReturnDeclarationDto declarationDto, SyntaxNode syntax)
    {
        if (declarationDto.ManipulationFormat.Contains(syntax.ToFullString())) return;

        declarationDto.ManipulationFormat = syntax.ToFullString();
        declarationDto.HasVoid = false;
    }
}