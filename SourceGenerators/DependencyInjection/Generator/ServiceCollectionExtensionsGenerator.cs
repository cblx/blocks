using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Cblx.Blocks.SourceGenerators.DependencyInjection;

[Generator]
public class ServiceCollectionExtensionsGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        var syntaxReceiver = context.SyntaxReceiver as SyntaxReceiver;
        string assemblyName = context.Compilation.AssemblyName;
        string addServicesName = GetAddServicesMethodName(assemblyName);
        var additionalNamespaces = "";
        var addAllServicesMethodDeclaration = "";
        SetupServicesEntry();
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
                    {{ToAddServices(context, syntaxReceiver.AnnotatedServices)}}
                    return services;
                }{{addAllServicesMethodDeclaration}}
            }
            """;

        context.AddSource("ServiceCollectionExtensions.g.cs", source);


        void SetupServicesEntry()
        {
            var attributes = context.Compilation.Assembly.GetAttributes();
            var servicesEntryAttribute = attributes.FirstOrDefault(attr => attr.AttributeClass.Name == "ServicesEntryAttribute");
            if (servicesEntryAttribute == null) { return; }
            var includeArgument = servicesEntryAttribute.NamedArguments.FirstOrDefault(a => a.Key == "Include").Value.Value?.ToString();
            if (string.IsNullOrEmpty(includeArgument)) { return; }
            var excludeArgument = servicesEntryAttribute.NamedArguments.FirstOrDefault(a => a.Key == "Exclude").Value.Value?.ToString();
            var references = context.Compilation.References.Select(r => r.Display).ToArray();
            var includedReferences = references.Where(r => Regex.IsMatch(r, includeArgument));
            if (!string.IsNullOrEmpty(excludeArgument))
            {
                var excludedReferences = references.Where(r => Regex.IsMatch(r, excludeArgument));
                includedReferences = includedReferences.Except(excludedReferences).ToArray();
            }

            additionalNamespaces = Environment.NewLine + string.Join(Environment.NewLine, includedReferences.Select(r => $"using {r};"));
            var addServicesLines = Environment.NewLine + string.Join(Environment.NewLine, includedReferences.Select(r => $"        services.{GetAddServicesMethodName(r)}();"));
            addAllServicesMethodDeclaration = $$"""


                    public static IServiceCollection AddAllServices(this IServiceCollection services)
                    {
                        services.{{addServicesName}}();{{addServicesLines}}
                        return services;
                    }
                """;
        }

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