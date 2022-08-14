using Cblx.Blocks.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;

namespace Cblx.Blocks.Factories;

internal static class HandlerReturnDeclarationFactory
{
    public static HandlerReturnDeclaration Create(MethodDeclarationSyntax methodDeclarationSyntax)
    {
        var returnMethodTree = methodDeclarationSyntax.ReturnType.DescendantNodesAndSelf().ToArray();
        var returnDeclaration = IdentifyAndProcessMethodReturnTree(returnMethodTree);

        return new HandlerReturnDeclaration(
            returnDeclaration.TypeName,
            methodDeclarationSyntax.ReturnType.ToFullString(),
            returnDeclaration.ManipulationFormat,
            returnDeclaration.HasVoid,
            returnDeclaration.HasAsync);

    }

    private static ReturnDeclaration IdentifyAndProcessMethodReturnTree(SyntaxNode[] tree)
    {
        var returnDeclaration = new ReturnDeclaration();

        foreach (var node in tree)
        {
            switch (node)
            {
                case GenericNameSyntax syntax: returnDeclaration.ProcessGenericNameSyntax(syntax); break;
                case IdentifierNameSyntax syntax: returnDeclaration.ProcessIdentifierNameSyntax(syntax); break;
            }   
        }

        return returnDeclaration;
    }

    private static void ProcessArrayTypeSyntax(this ReturnDeclaration declaration, ArrayTypeSyntax syntax)
    {
        throw new NotImplementedException();
    }

    private static void ProcessGenericNameSyntax(this ReturnDeclaration declaration, GenericNameSyntax syntax)
    {
        var name = syntax.Identifier.Text.Trim();

        declaration.TypeName = name;
        declaration.HasAsync = name is "Task" or "ValueTask";
        declaration.HasVoid = true;
    }

    private static void ProcessIdentifierNameSyntax(this ReturnDeclaration declaration, IdentifierNameSyntax syntax)
    {
        var name = syntax.Identifier.Text.Trim();

        declaration.TypeName = name;
        declaration.ManipulationFormat = name;
        declaration.HasVoid = false;
    }



    protected record ReturnDeclaration
    {
        public string TypeName { get; set; } = default!;
        public string ManipulationFormat { get; set; } = default!;

        public bool HasVoid { get; set; }
        public bool HasAsync { get; set; }

        
    }
}
