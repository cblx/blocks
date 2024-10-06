using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Cblx.Blocks.Analyzers.Endpoints;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class EndpointExecuteAsyncAnalyzer : DiagnosticAnalyzer
{
    private const string EndpointBase = "EndpointBase";
    private const string MethodName = "ExecuteAsync";
    
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        ImmutableArray.Create(
            DiagnosticDescriptors.EndpointShouldContainExecuteAsyncMethod,
            DiagnosticDescriptors.ExecuteAsyncMethodShouldBeStatic,
            DiagnosticDescriptors.ExecuteAsyncMethodShouldBeInternal);
    
    public override void Initialize(AnalysisContext context)
    {
        // You must call this method to avoid analyzing generated code.
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);

        // You must call this method to enable the Concurrent Execution.
        context.EnableConcurrentExecution();

        // Subscribe to semantic (compile time) action invocation, e.g. method invocation.
        context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.ClassDeclaration);
    }

    private void AnalyzeNode(SyntaxNodeAnalysisContext context)
    {
        var options = CreateOptions(context.Options.AnalyzerConfigOptionsProvider.GetOptions(context.Node.SyntaxTree));
        
        if (context.Node is not ClassDeclarationSyntax classDeclaration) return;
        
        var semanticModel = context.SemanticModel;

        var classSymbol = semanticModel.GetDeclaredSymbol(classDeclaration);
        
        if(classSymbol is null || classSymbol.IsAbstract || classSymbol.Name is EndpointBase || !InheritsFromEndpointBase(classSymbol.BaseType)) return;
        
        var executeAsyncMethod = classSymbol.GetMembers().OfType<IMethodSymbol>().FirstOrDefault(m => m.Name == MethodName);

        switch (executeAsyncMethod)
        {
            case null:
                context.ReportDiagnostic(CreateDiagnostic(classDeclaration, DiagnosticDescriptors.EndpointShouldContainExecuteAsyncMethod));
                break;
            case { IsStatic: false }:
                context.ReportDiagnostic(CreateDiagnostic(executeAsyncMethod, DiagnosticDescriptors.ExecuteAsyncMethodShouldBeStatic));
                break;
            case { DeclaredAccessibility: not Accessibility.Internal } when options.UseDeclaredAccessibilityAnalyzer: 
                context.ReportDiagnostic(CreateDiagnostic(executeAsyncMethod, DiagnosticDescriptors.ExecuteAsyncMethodShouldBeInternal));
                break;
        }
    }

    private static EndpointExecuteAsyncOptions CreateOptions(AnalyzerConfigOptions options)
    {
        var endpointOptions = new EndpointExecuteAsyncOptions ();
        
        if (options.TryGetValue(EndpointExecuteAsyncOptions.DeclaredAccessibilityAnalyzerName, out var value))
        {
            endpointOptions.UseDeclaredAccessibilityAnalyzer = string.IsNullOrEmpty(value) || bool.Parse(value);
        }
        
        return endpointOptions;
    }

    private static Diagnostic CreateDiagnostic(IMethodSymbol methodSymbol, DiagnosticDescriptor descriptor)
    {
        return Diagnostic.Create(
            descriptor, 
            methodSymbol.Locations[0], 
            methodSymbol.Name);
    }
    
    private static Diagnostic CreateDiagnostic(ClassDeclarationSyntax classDeclaration, DiagnosticDescriptor descriptor)
    {
        return Diagnostic.Create(
            descriptor, 
            classDeclaration.Identifier.GetLocation(), 
            classDeclaration.Identifier.Text);
    }

    private static bool InheritsFromEndpointBase(INamedTypeSymbol? namedTypeSymbol)
    {
        while (namedTypeSymbol is not null)
        {
            if (namedTypeSymbol.Name == EndpointBase)
            {
                return true;
            }
            namedTypeSymbol = namedTypeSymbol.BaseType;
        }
        
        return false;
    }
}