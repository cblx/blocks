namespace Cblx.Blocks.Ids.Generators;

[Generator]
public class TypedIdGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        if (context.SyntaxReceiver is not SyntaxReceiver syntaxReceiver) return;
        
        foreach (var classDeclarationSyntax in syntaxReceiver.Classes)
        {
            var model = context.Compilation.GetSemanticModel(classDeclarationSyntax.SyntaxTree);
            if(model.GetDeclaredSymbol(classDeclarationSyntax) is not ITypeSymbol symbol) continue;
            
            var name = $"{symbol.Name}Id";
            var namespaceName = symbol.ContainingNamespace.ToDisplayString();
            var source = TypedIdTemplate.Create(name, namespaceName);
            
            context.AddSource($"{name}.g.cs", source);
        }
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
    }
}