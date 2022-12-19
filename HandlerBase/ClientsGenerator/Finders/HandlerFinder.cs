using Cblx.Blocks.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace Cblx.Blocks.Finders;

internal sealed class HandlerFinder : ISyntaxReceiver
{
    private readonly List<InterfaceDeclarationSyntax> _handlers;

    public HandlerFinder() => _handlers = new List<InterfaceDeclarationSyntax>();

    public IReadOnlyCollection<InterfaceDeclarationSyntax> Handlers => _handlers;

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is not InterfaceDeclarationSyntax interfaceSyntax) return;
        if (!AttributeHelper.ContainsGenerateClientAttribute(interfaceSyntax)) return;

        _handlers.Add(interfaceSyntax);
    }
}
