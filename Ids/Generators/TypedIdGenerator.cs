using System.Text;

namespace Cblx.Blocks.Ids.Generators;

[Generator]
public class TypedIdGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        if (context.SyntaxReceiver is not SyntaxReceiver syntaxReceiver) return;

        var ids = new StringBuilder();
        foreach (var classDeclarationSyntax in syntaxReceiver.Classes)
        {
            var model = context.Compilation.GetSemanticModel(classDeclarationSyntax.SyntaxTree);
            if(model.GetDeclaredSymbol(classDeclarationSyntax) is not ITypeSymbol symbol) continue;
            
            var name = $"{symbol.Name}Id";
            var namespaceName = symbol.ContainingNamespace.ToDisplayString();
            var source = TypedIdTemplate.Create(name, namespaceName);
            
            context.AddSource($"{name}.g.cs", source);
            ids.AppendLine(TypedIdHelperTemplate.CreateTypeOfLine(namespaceName, name));
        }
        
        if(ids.Length <= 0) return; 
        
        var idsSource = TypedIdHelperTemplate.Create("Cblx.Blocks", ids.ToString());
        context.AddSource("TypedIdHelper.g.cs", idsSource);
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
    }
}