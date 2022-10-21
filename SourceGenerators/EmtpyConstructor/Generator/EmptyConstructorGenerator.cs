using Cblx.Blocks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace EmptyConstructor
{
    [Generator]
    public class EmptyConstructorGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            if (!(context.SyntaxReceiver is SyntaxReceiver syntaxReceiver)) return;

            foreach (ClassDeclarationSyntax classDeclarationSyntax in syntaxReceiver.Classes)
            {
                SemanticModel model = context.Compilation.GetSemanticModel(classDeclarationSyntax.SyntaxTree);
                if (model.GetDeclaredSymbol(classDeclarationSyntax) is not ITypeSymbol symbol) { continue; }
                string source = $$"""
                    using System.Diagnostics.CodeAnalysis;
                        
                    namespace {{symbol.ContainingNamespace}};
                    public partial class {{symbol.Name}}
                    {
                    #pragma warning disable CS8618
                        [Obsolete("This constructor is reserved for serialization only. Do not use it.")]
                        [ExcludeFromCodeCoverage]
                        public {{symbol.Name}}(){}
                    #pragma warning restore CS8618
                    }
                    """;
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
            private static readonly string _attributeName = nameof(HasObsoleteEmptyConstructorAttribute).Replace("Attribute", "");

            public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
            {
                if (!(syntaxNode is ClassDeclarationSyntax classDeclarationSyntax))
                {
                    return;
                }

                bool hasHasObsoleteEmptyConstructorAttribute = classDeclarationSyntax
                    .AttributeLists
                    .SelectMany(list => list.Attributes)
                    .Any(a => a.Name.ToString() == _attributeName);
                //if (attrListText.Contains(_attributeName))
                if (hasHasObsoleteEmptyConstructorAttribute)
                {
                    Classes.Add(classDeclarationSyntax);
                }
            }
        }
    }
}