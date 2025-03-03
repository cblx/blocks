//using Microsoft.CodeAnalysis;
//using Microsoft.CodeAnalysis.CSharp.Syntax;
//using Microsoft.CodeAnalysis.Text;
//using System;
//using System.Linq;
//using System.Text;

//namespace Cblx.Blocks.SourceGenerators.Dto;

////[Generator]
//public class DtoGenerator /*: IIncrementalGenerator*/
//{
//    public void Initialize(IncrementalGeneratorInitializationContext context)
//    {
//        var pipeline = context.SyntaxProvider.ForAttributeWithMetadataName(
//            fullyQualifiedMetadataName: "Cblx.Blocks.DtoOfAttribute`1",
//            predicate: static (syntaxNode, cancellationToken) => syntaxNode is ClassDeclarationSyntax,
//            // When returning a value type or record, or a custom comparer, the generation can be optimized.
//            // more: https://andrewlock.net/creating-a-source-generator-part-9-avoiding-performance-pitfalls-in-incremental-generators/#3-use-a-value-type-data-model-or-records-or-a-custom-comparer-
//            transform: static (context, cancellationToken) =>
//            {
//                var sourceClass = context.Attributes[0].AttributeClass!.TypeArguments[0];
//                var annotatedClass = (context.TargetSymbol as INamedTypeSymbol)!;
//                var ignoredProps = annotatedClass.GetAttributes()
//                    .Where(a => new string[] { nameof(DtoIgnoreAttribute) }.Contains(a.AttributeClass?.Name))
//                    .SelectMany(a => a.NamedArguments.Select(c => c.Value.Value?.ToString()))
//                    .ToArray();
//                var properties = sourceClass.GetAllProperties().Where(property => !ignoredProps.Contains(property.Name)).Select(property => {
//                    var typeAsNullable = property.Type.IsReferenceType ? $"{property.Type.WithNullableAnnotation(NullableAnnotation.Annotated)}"
//                                                                           : property.Type.Name != "Nullable" ? $"{property.Type}?" : $"{property.Type}";
//                    return $"    public {typeAsNullable} {property.Name} {{ get; set; }}";
//                }).ToArray();
//                return new Model
//                {
//                    // Note: this is a simplified example. You will also need to handle the case where the type is in a global namespace, nested, etc.
//                    Namespace = annotatedClass.ContainingNamespace?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat.WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle.Omitted)) ?? "",
//                    ClassName = annotatedClass.Name,
//                    Properties = new EquatableArray<string>(properties)
//                };
//            }
//        ).WithTrackingName("Extraction");

//        context.RegisterSourceOutput(pipeline, static (context, model) =>
//        {
//            var stringBuilder = new StringBuilder();
//            stringBuilder.AppendLine("#nullable enable");
//            stringBuilder.AppendLine($"namespace {model.Namespace};");
//            stringBuilder.AppendLine($"partial class {model.ClassName}");
//            stringBuilder.AppendLine("{");
//            foreach (var property in model.Properties)
//            {
//                stringBuilder.AppendLine(property);
//            }
//            stringBuilder.AppendLine("}");

//            context.AddSource($"{model.ClassName}.Dto.g.cs", SourceText.From(stringBuilder.ToString(), Encoding.UTF8));
//        });
//    }

//    private record Model
//    {
//        public string Namespace { get; set; } = default!;
//        public string ClassName { get; set; } = default!;
//        public EquatableArray<string> Properties { get; set; } = default!;
//    }


//    //public void Execute(GeneratorExecutionContext context)
//    //{
//    //    if (context.SyntaxReceiver is not SyntaxReceiver syntaxReceiver) return;

//    //    foreach (ClassDeclarationSyntax classDeclarationSyntax in syntaxReceiver.Classes)
//    //    {
//    //        SemanticModel model = context.Compilation.GetSemanticModel(classDeclarationSyntax.SyntaxTree);
//    //        if (model.GetDeclaredSymbol(classDeclarationSyntax) is not ITypeSymbol symbol) { continue; }


//    //        var attrs = symbol.GetAttributes()
//    //            .Where(a => new string[] {
//    //                    nameof(DtoOfAttribute<object>),
//    //                    nameof(DtoIgnoreAttribute),
//    //            }.Contains(a.AttributeClass.Name))
//    //            .ToArray();
//    //        var dtoOfAttr = attrs.FirstOrDefault(a => a.AttributeClass.Name == nameof(DtoOfAttribute<object>));
//    //        var dtoIgnoreAttrs = attrs.Where(a => a.AttributeClass.Name == nameof(DtoIgnoreAttribute)).ToArray();
//    //        if (dtoOfAttr is null) { continue; }
//    //        var properties = dtoOfAttr.AttributeClass.TypeArguments[0].GetAllProperties();

//    //        var stringBuilder = new StringBuilder();
//    //        stringBuilder.AppendLine("#nullable enable");
//    //        stringBuilder.AppendLine($"namespace {symbol.ContainingNamespace};");
//    //        stringBuilder.AppendLine($"partial class {symbol.Name}");
//    //        stringBuilder.AppendLine("{");
//    //        foreach (var property in properties)
//    //        {
//    //            if (dtoIgnoreAttrs.Any(a => a.NamedArguments.Any(c => c.Value.Value?.ToString() == property.Name)))
//    //            {
//    //                continue;
//    //            }
//    //            var typeAsNullable = property.Type.IsReferenceType ? $"{property.Type.WithNullableAnnotation(NullableAnnotation.Annotated)}"
//    //                                                                   : property.Type.Name != "Nullable" ? $"{property.Type}?" : $"{property.Type}";
//    //            stringBuilder.AppendLine($"    public {typeAsNullable} {property.Name} {{ get; set; }}");
//    //        }

//    //        stringBuilder.AppendLine("}");

//    //        context.AddSource($"{symbol.Name}.Dto.g.cs", stringBuilder.ToString());
//    //    }
//    //}



//    //public void Initialize(GeneratorInitializationContext context)
//    //{
//    //    //#if DEBUG
//    //    //        if (!Debugger.IsAttached)
//    //    //        {
//    //    //            Debugger.Launch();
//    //    //        }
//    //    //#endif
//    //    context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
//    //}

//    //class SyntaxReceiver : ISyntaxReceiver
//    //{
//    //    public List<ClassDeclarationSyntax> Classes { get; } = new List<ClassDeclarationSyntax>();
//    //    private static readonly string _dtoOfAttributeName = nameof(DtoOfAttribute<object>).Replace("Attribute", "");
//    //    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
//    //    {
//    //        if (syntaxNode is not ClassDeclarationSyntax classDeclarationSyntax)
//    //        {
//    //            return;
//    //        }

//    //        bool hasDtoFromAttribute = classDeclarationSyntax
//    //                                                            .AttributeLists
//    //                                                            .SelectMany(list => list.Attributes)
//    //                                                            .Any(a => a.Name.ToString().StartsWith(_dtoOfAttributeName));
//    //        if (hasDtoFromAttribute)
//    //        {
//    //            Classes.Add(classDeclarationSyntax);
//    //        }
//    //    }
//    //}
//}