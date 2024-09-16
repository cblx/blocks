using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Cblx.Blocks.Analyzers.Endpoints;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(EndpointInternalExecuteAsyncProvider)), Shared]
public class EndpointInternalExecuteAsyncProvider : CodeFixProvider
{
    public sealed override ImmutableArray<string> FixableDiagnosticIds { get; } =
        ImmutableArray.Create(DiagnosticDescriptors.DiagnosticExecuteAsyncMethodShouldBeInternalId);
    
    public override FixAllProvider? GetFixAllProvider() => null;
    
    public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

        foreach (var diagnostic in context.Diagnostics)
        {
            var diagnosticSpan = diagnostic.Location.SourceSpan;
            var diagnosticNode = root?.FindNode(diagnosticSpan);

            if (diagnosticNode is MethodDeclarationSyntax methodDeclaration)
            {
                context.RegisterCodeFix(
                    CodeAction.Create(
                        title: Resources.CBLX0003CodeFixTitle,
                        createChangedSolution: c => MakeOrChangeMethodInternalAsync(context.Document, methodDeclaration, c),
                        equivalenceKey: nameof(Resources.CBLX0002CodeFixTitle)),
                    diagnostic);
            }
        }
    }

    private static async Task<Solution> MakeOrChangeMethodInternalAsync(Document document, MethodDeclarationSyntax methodDeclaration, CancellationToken cancellationToken)
    {
        var modifiers = methodDeclaration.Modifiers;
        
        // Remove access modifier
        modifiers = RemoveAccessModifier(modifiers);
        
        // Add internal access modifier
        var internalModifier = SyntaxFactory.Token(SyntaxKind.InternalKeyword);
        modifiers = SyntaxFactory.TokenList(internalModifier).AddRange(modifiers);
        
        var newMethodDeclaration = methodDeclaration.WithModifiers(modifiers);
        
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        var newRoot = root!.ReplaceNode(methodDeclaration, newMethodDeclaration);
        var newDocument = document.WithSyntaxRoot(newRoot);
        return newDocument.Project.Solution;
        
    }

    private static SyntaxTokenList RemoveAccessModifier(SyntaxTokenList modifiers)
    {
        var accessModifier = modifiers.FirstOrDefault(mod => 
            mod.IsKind(SyntaxKind.PublicKeyword) ||
            mod.IsKind(SyntaxKind.InternalKeyword) ||
            mod.IsKind(SyntaxKind.PrivateKeyword) ||
            mod.IsKind(SyntaxKind.ProtectedKeyword));
        
        return accessModifier == default ? modifiers : modifiers.Remove(accessModifier);
    }
}