using Cblx.Blocks.Exceptions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Cblx.Blocks.Helpers;

public static class CodeHelpers
{
    private static GeneratorExecutionContext? _context;

    private static GeneratorExecutionContext Context => _context ?? throw new ConfigurationException("GeneratorExecutionContext is required");
    private static Compilation Compilation => _context!.Value.Compilation;

    public static void ChangeGeneratorExecutionContext(GeneratorExecutionContext context)
    {
        _context = context;
    }

    public static void RemoveGeneratorExecutionContext()
    {
        _context = null;
    }

    public static string? GetNamespace(TypeSyntax typeSyntax)
    {
        var semantic = Compilation.GetSemanticModel(typeSyntax.SyntaxTree);
        return GetNamespace(typeSyntax, semantic)?.ToString();
    }

    private static INamespaceSymbol? GetNamespace(SyntaxNode node, SemanticModel semanticModel)
    {
        var name =  semanticModel.GetTypeInfo(node).Type?.ContainingNamespace;
        return name;
    }

    public static IAssemblySymbol GetAssemblySymbol()
    {
        return Compilation.Assembly;
    }

    public static string? GetAssemblyName()
    {
        return Compilation.AssemblyName;
    }

    public static ISymbol? GetDeclaredSymbol(SyntaxNode syntaxNode)
    {
        var semanticModel = Compilation.GetSemanticModel(syntaxNode.SyntaxTree);
        return semanticModel.GetDeclaredSymbol(syntaxNode, Context.CancellationToken);
    }

    public static void AddSource(string hintName, string source)
    {
        Context.AddSource(hintName, source);
    }
}
