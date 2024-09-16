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

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(EndpointStaticExecuteAsyncProvider)), Shared]
public class EndpointStaticExecuteAsyncProvider : CodeFixProvider
{
    public sealed override ImmutableArray<string> FixableDiagnosticIds { get; } =
        ImmutableArray.Create(DiagnosticDescriptors.DiagnosticExecuteAsyncMethodShouldBeStaticId);

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
                        title: Resources.CBLX0002CodeFixTitle,
                        createChangedSolution: c => MakeMethodStaticAsync(context.Document, methodDeclaration, c),
                        equivalenceKey: nameof(Resources.CBLX0002CodeFixTitle)),
                    diagnostic);
            }
        }
    }
    
    private static async Task<Solution> MakeMethodStaticAsync(Document document, MethodDeclarationSyntax methodDeclaration, CancellationToken cancellationToken)
    {
        var modifiers = methodDeclaration.Modifiers;

        var accessModifier = modifiers.FirstOrDefault(mod => 
            mod.IsKind(SyntaxKind.PublicKeyword) ||
            mod.IsKind(SyntaxKind.InternalKeyword) ||
            mod.IsKind(SyntaxKind.PrivateKeyword) ||
            mod.IsKind(SyntaxKind.ProtectedKeyword));

        var staticModifier = SyntaxFactory.Token(SyntaxKind.StaticKeyword);
        
        if (accessModifier == default)
        {
            modifiers = SyntaxFactory.TokenList(staticModifier).AddRange(modifiers);
        }
        else
        {
            var index = modifiers.IndexOf(accessModifier);
            modifiers = modifiers.Insert(index + 1, staticModifier);
        }

        var newMethodDeclaration = methodDeclaration.WithModifiers(modifiers);

        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        var newRoot = root!.ReplaceNode(methodDeclaration, newMethodDeclaration);
        var newDocument = document.WithSyntaxRoot(newRoot);
        return newDocument.Project.Solution;
    }
}