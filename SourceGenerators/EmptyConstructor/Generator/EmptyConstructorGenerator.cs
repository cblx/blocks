using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Cblx.Blocks.SourceGenerators.EmptyConstructor;

[Generator]
public class EmptyConstructorGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        if (context.SyntaxReceiver is not SyntaxReceiver syntaxReceiver) return;

        foreach (ClassDeclarationSyntax classDeclarationSyntax in syntaxReceiver.Classes)
        {
            SemanticModel model = context.Compilation.GetSemanticModel(classDeclarationSyntax.SyntaxTree);
            if (model.GetDeclaredSymbol(classDeclarationSyntax) is not ITypeSymbol symbol) { continue; }
            

            var attr = symbol.GetAttributes()
                .Where(a => new string[] {
                        nameof(HasObsoleteEmptyConstructorAttribute),
                        nameof(HasPrivateEmptyConstructorAttribute),
                        nameof(HasProtectedEmptyConstructorAttribute)
                }.Contains(a.AttributeClass.Name))
                .FirstOrDefault();

            if(attr is null) { continue; }
            var accessor = attr.AttributeClass.Name switch
            {
                nameof(HasObsoleteEmptyConstructorAttribute) => "public",
                nameof(HasPrivateEmptyConstructorAttribute) => "private",
                nameof(HasProtectedEmptyConstructorAttribute) => "protected",
                _ => "public"
            };
            string obsoletAttribute = accessor == "public" ? ", Obsolete(\"This constructor is reserved for deserialization only. Do not use it.\")" : "";
            var hasPrimaryConstructor = classDeclarationSyntax.ParameterList is not null;
            string source;
            if (hasPrimaryConstructor)
            {
                var defaults = string.Join(", ", classDeclarationSyntax.ParameterList
                                                     .Parameters
                                                     .Select(p => "default!"));
                source = $$"""
                using System.Diagnostics.CodeAnalysis;
                        
                namespace {{symbol.ContainingNamespace}};
                partial class {{symbol.Name}}
                {
                #pragma warning disable CS8618
                    [ExcludeFromCodeCoverage{{obsoletAttribute}}]
                    {{accessor}} {{symbol.Name}}() : this({{defaults}}) {}
                #pragma warning restore CS8618
                }
                """;
            }
            else
            {
                source = $$"""
                using System.Diagnostics.CodeAnalysis;
                        
                namespace {{symbol.ContainingNamespace}};
                partial class {{symbol.Name}}
                {
                #pragma warning disable CS8618
                    [ExcludeFromCodeCoverage{{obsoletAttribute}}]
                    {{accessor}} {{symbol.Name}}(){}
                #pragma warning restore CS8618
                }
                """;
            }
            context.AddSource($"{symbol.Name}.g.cs", source);
        }
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        //#if DEBUG
        //            if (!Debugger.IsAttached)
        //            {
        //                Debugger.Launch();
        //            }
        //#endif 
        context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
    }

    class SyntaxReceiver : ISyntaxReceiver
    {
        public List<ClassDeclarationSyntax> Classes { get; } = new List<ClassDeclarationSyntax>();
        private static readonly string _obsoleteAttributeName = nameof(HasObsoleteEmptyConstructorAttribute).Replace("Attribute", "");
        private static readonly string _privateAttributeName = nameof(HasPrivateEmptyConstructorAttribute).Replace("Attribute", "");
        private static readonly string _protectedAttributeName = nameof(HasProtectedEmptyConstructorAttribute).Replace("Attribute", "");

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is not ClassDeclarationSyntax classDeclarationSyntax)
            {
                return;
            }

            bool hasHasObsoleteEmptyConstructorAttribute = classDeclarationSyntax
                .AttributeLists
                .SelectMany(list => list.Attributes)
                .Any(a => a.Name.ToString() == _obsoleteAttributeName 
                       || a.Name.ToString() == _privateAttributeName
                       || a.Name.ToString() == _protectedAttributeName);
            if (hasHasObsoleteEmptyConstructorAttribute)
            {
                Classes.Add(classDeclarationSyntax);
            }
        }
    }
}