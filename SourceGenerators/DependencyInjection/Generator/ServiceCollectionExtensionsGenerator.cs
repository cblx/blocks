using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;

namespace Cblx.Blocks.SourceGenerators.DependencyInjection;

[Generator]
public class ServiceCollectionExtensionsGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        var syntaxReceiver = context.SyntaxReceiver as SyntaxReceiver;
        string assemblyName = context.Compilation.AssemblyName;
        string addServicesName = GetAddServicesMethodName(assemblyName);
        var attributes = context.Compilation.Assembly.GetAttributes();
        var servicesEntryAttribute = attributes.FirstOrDefault(attr => attr.AttributeClass.Name == "ServicesEntryAttribute");
        var additionalNamespaces = "";
        var addAllServicesMethodDeclaration = "";
        if (servicesEntryAttribute != null)
        {
            var prefix = servicesEntryAttribute.ConstructorArguments.First().Value.ToString();
            var referencesNames = context.Compilation.References.Where(r => r.Display.StartsWith(prefix)).ToArray();
            additionalNamespaces = Environment.NewLine + string.Join(Environment.NewLine, referencesNames.Select(r => $"using {r.Display};"));
            var addServicesLines = Environment.NewLine + string.Join(Environment.NewLine, referencesNames.Select(r => $"        services.{GetAddServicesMethodName(r.Display)}();"));
            addAllServicesMethodDeclaration = $$"""


                    public static IServiceCollection AddAllServices(this IServiceCollection services)
                    {
                        services.{{addServicesName}}();{{addServicesLines}}
                        return services;
                    }
                """;
        }
        string source = $$"""
            // Auto-generated code
            using Microsoft.Extensions.DependencyInjection;
            using System.Diagnostics.CodeAnalysis;{{additionalNamespaces}}
            namespace {{assemblyName}};
            [ExcludeFromCodeCoverage]
            public static partial class ServiceCollectionExtensions
            {
                public static IServiceCollection {{addServicesName}}(this IServiceCollection services)
                {
                    {{ToAddServices(context,syntaxReceiver.AnnotatedServices)}}
                    return services;
                }{{addAllServicesMethodDeclaration}}
            }
            """;

        context.AddSource("ServiceCollectionExtensions.g.cs", source);
    }

    private static string GetAddServicesMethodName(string assemblyName) => $"Add{assemblyName.Replace(".", "")}Services";
    
    string ToAddServices(GeneratorExecutionContext context, List<ClassDeclarationSyntax> annotatedServices)
    {
        var lines = new List<string>();
        foreach (ClassDeclarationSyntax classDeclarationSyntax in annotatedServices)
        {
            SemanticModel model = context.Compilation.GetSemanticModel(classDeclarationSyntax.SyntaxTree);
            ISymbol symbol = model.GetDeclaredSymbol(classDeclarationSyntax);
            IEnumerable<AttributeData> attrs = symbol.GetAttributes()
                .Where(a => new string[] {
                        "ScopedAttribute",
                        "TransientAttribute",
                        "SingletonAttribute"
                }.Contains(a.AttributeClass.Name));

            foreach (var attr in attrs.Select(attr => attr.AttributeClass))
            {
                string scope = attr.Name.Replace("Attribute", "");
                object type = attr.TypeArguments.Select(typeSimbol => typeSimbol).FirstOrDefault();
                lines.Add(type is null
                    ? $"services.Add{scope}<{symbol}>();"
                    : $"services.Add{scope}<{type}, {symbol}>();");
            }
        }
        return string.Join($"{Environment.NewLine}        ", lines.ToArray());  
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
    }
}

class SyntaxReceiver : ISyntaxReceiver
{
    public List<ClassDeclarationSyntax> AnnotatedServices { get; } = new List<ClassDeclarationSyntax>();

    private static readonly string _scopedAttributeName = nameof(ScopedAttribute<object>).Replace("Attribute", "");
    private static readonly string _singletonAttributeName = nameof(SingletonAttribute<object>).Replace("Attribute", "");
    private static readonly string _transientAttributeName = nameof(TransientAttribute<object>).Replace("Attribute", "");
    
    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax)
        {
            string attrListText = classDeclarationSyntax.AttributeLists.ToString();
            if (attrListText.Contains(_scopedAttributeName)
                || attrListText.Contains(_singletonAttributeName)
                || attrListText.Contains(_transientAttributeName)
            )
            {
                AnnotatedServices.Add(classDeclarationSyntax);
            }
        }
    }
}