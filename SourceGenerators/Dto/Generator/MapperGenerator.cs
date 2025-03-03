//using Microsoft.CodeAnalysis;
//using Microsoft.CodeAnalysis.CSharp.Syntax;
//using Microsoft.CodeAnalysis.Text;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Cblx.Blocks.SourceGenerators.Dto;

////[Generator]
//public class MapperGenerator /*: IIncrementalGenerator*/
//{
//    public void Initialize(IncrementalGeneratorInitializationContext context)
//    {
//        var pipeline = context.SyntaxProvider.ForAttributeWithMetadataName(
//            fullyQualifiedMetadataName: "Cblx.Blocks.MapperAttribute",
//            predicate: static (syntaxNode, cancellationToken) => syntaxNode is BaseMethodDeclarationSyntax,
//            // When returning a value type or record, or a custom comparer, the generation can be optimized.
//            // more: https://andrewlock.net/creating-a-source-generator-part-9-avoiding-performance-pitfalls-in-incremental-generators/#3-use-a-value-type-data-model-or-records-or-a-custom-comparer-
//            transform: static (context, cancellationToken) =>
//            {
//                var methodSymbol = (context.TargetSymbol as IMethodSymbol)!;
//                var containingType = methodSymbol.ContainingType;
//                var firstParameter = methodSymbol.Parameters.FirstOrDefault();
//                var sourceType = firstParameter?.Type ?? containingType;
//                var targetType = methodSymbol.ReturnType;
//                var targetConstructor = targetType.GetMembers()
//                                                  .Where(m => m is IMethodSymbol { MethodKind: MethodKind.Constructor, DeclaredAccessibility: Accessibility.Public })
//                                                  .Cast<IMethodSymbol>()
//                                                  .OrderBy(m => m.Parameters.Length)
//                                                  .FirstOrDefault();

//                var shouldGenerate = !methodSymbol.ReturnsVoid && methodSymbol.Parameters.Length is 0 or 1 && targetConstructor != null;
//                var targetProperties = targetType.GetMembers().Where(m => m.Kind == SymbolKind.Property).Cast<IPropertySymbol>().Where(p => p.SetMethod is { DeclaredAccessibility: Accessibility.Public }).ToArray();
//                var sourceProperties = sourceType.GetMembers().Where(m => m.Kind == SymbolKind.Property).Cast<IPropertySymbol>().Where(p => p.GetMethod is { DeclaredAccessibility: Accessibility.Public }).ToArray();

//                var constructorParameters = targetConstructor?.Parameters ?? [];

//                var model = new Model
//                {
//                    // Note: this is a simplified example. You will also need to handle the case where the type is in a global namespace, nested, etc.
//                    Namespace = containingType.ContainingNamespace?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat.WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle.Omitted)) ?? "",
//                    ClassName = containingType.Name,
//                    MethodName = context.TargetSymbol.Name,
//                    ShouldGenerate = shouldGenerate,
//                    ReturnTypeName = methodSymbol.ReturnType.ToString(),
//                    MethodModifiers = $"{methodSymbol.DeclaredAccessibility.ToString().ToLower()}{(methodSymbol.IsStatic ? " static" : "")}",
//                    SourceName = firstParameter?.Name ?? "this",
//                    SourceType = sourceType.ToString(),
//                    ConstructorParameters = new EquatableArray<Argument>(constructorParameters.Select(p => new Argument { 
//                        Name = p.Name, 
//                        Nullable = p.NullableAnnotation == NullableAnnotation.Annotated,
//                        IsReferenceType = p.Type.IsReferenceType }).ToArray()),
//                    SourceProperties = new EquatableArray<Argument>(sourceProperties.Select(p => new Argument { 
//                        Name = p.Name, 
//                        Nullable = p.NullableAnnotation == NullableAnnotation.Annotated,
//                        IsReferenceType = p.Type.IsReferenceType
//                    }).ToArray()),
//                    TargetProperties = new EquatableArray<Argument>(targetProperties.Select(p => new Argument { 
//                        Name = p.Name, 
//                        Nullable = p.NullableAnnotation == NullableAnnotation.Annotated,
//                        IsReferenceType = p.Type.IsReferenceType
//                    }).ToArray())
//                };
//                if (shouldGenerate)
//                {
//                }
//                return model;
//            }
//        ).WithTrackingName("Extraction");

//        context.RegisterSourceOutput(pipeline, static (context, model) =>
//        {
//            if (!model.ShouldGenerate) { return; }
//            var stringBuilder = new StringBuilder();
//            stringBuilder.AppendLine("#nullable enable");
//            stringBuilder.AppendLine($"namespace {model.Namespace};");
//            stringBuilder.AppendLine($"partial class {model.ClassName}");
//            stringBuilder.AppendLine("{");
//            var parameter = model.SourceName != "this" ? $"{model.SourceType} {model.SourceName}" : "";
//            stringBuilder.AppendLine($"    {model.MethodModifiers} partial {model.ReturnTypeName} {model.MethodName}({parameter})");
//            stringBuilder.AppendLine("    {");
//            var constructorParameters = new List<string>();
//            foreach (var p in model.ConstructorParameters)
//            {
//                var sourceProperty = model.SourceProperties.FirstOrDefault(sp => sp.Name.Equals(p.Name, StringComparison.OrdinalIgnoreCase));
//                if(sourceProperty is null)
//                {
//                    constructorParameters.Add($"default!");
//                    continue;
//                }
//                var nullabilityDivergenceResolver = "";
//                if (!p.Nullable && sourceProperty.Nullable)
//                {
//                    nullabilityDivergenceResolver = p.IsReferenceType ? "!" : ".GetValueOrDefault()";
//                }
//                constructorParameters.Add($"{model.SourceName}.{sourceProperty.Name}{nullabilityDivergenceResolver}");
//            }
//            stringBuilder.AppendLine($"        return new {model.ReturnTypeName}({string.Join(", ", constructorParameters)}){(model.TargetProperties.Any() ? "" : ";")}");
//            if (model.TargetProperties.Any())
//            {
//                stringBuilder.AppendLine("        {");
//                foreach (var property in model.TargetProperties)
//                {
//                    var sourceProperty = model.SourceProperties.FirstOrDefault(p => p.Name.Equals(property.Name, StringComparison.OrdinalIgnoreCase));
//                    if (sourceProperty is null)
//                    {
//                        continue;
//                    }
//                    var nullabilityDivergenceResolver = "";
//                    if (!property.Nullable && sourceProperty.Nullable)
//                    {
//                        nullabilityDivergenceResolver = property.IsReferenceType ? "!" : ".GetValueOrDefault()";
//                    }
//                    stringBuilder.AppendLine($"            {property.Name} = {model.SourceName}.{property.Name}{nullabilityDivergenceResolver},");
//                }
//                stringBuilder.AppendLine("        };");
//            }
//            stringBuilder.AppendLine("    }");
//            stringBuilder.AppendLine("}");
//            context.AddSource($"{model.ClassName}.{model.MethodName}.Mapper.g.cs", SourceText.From(stringBuilder.ToString(), Encoding.UTF8));
//        });
//    }

//    private record Model
//    {
//        public string Namespace { get; set; } = default!;
//        public string ClassName { get; set; } = default!;
//        public string MethodName { get; set; } = default!;
//        public string ReturnTypeName { get; set; } = default!;
//        public string MethodModifiers { get; set; } = default!;
//        public bool ShouldGenerate { get; set; }
//        public string SourceName { get; set; } = default!;
//        public string SourceType { get; set; } = default!;
//        public EquatableArray<Argument> ConstructorParameters { get; set; } = default!;
//        public EquatableArray<Argument> SourceProperties { get; set; } = default!;
//        public EquatableArray<Argument> TargetProperties { get; set; } = default!;

//        //public EquatableArray<string> Properties { get; set; } = default!;
//    }

//    private record Argument
//    {
//        public string Name { get; set; } = default!;
//        public bool Nullable { get; set; }
//        public bool IsReferenceType { get; set; }
//    }

//    //public void Execute(GeneratorExecutionContext context)
//    //{
//    //    if (context.SyntaxReceiver is not SyntaxReceiver syntaxReceiver) return;

//    //    foreach (ClassDeclarationSyntax classDeclarationSyntax in syntaxReceiver.Classes)
//    //    {
//    //        SemanticModel model = context.Compilation.GetSemanticModel(classDeclarationSyntax.SyntaxTree);
//    //        if (model.GetDeclaredSymbol(classDeclarationSyntax) is not ITypeSymbol symbol) { continue; }

//    //        var stringBuilder = new StringBuilder();
//    //        stringBuilder.AppendLine("#nullable enable");
//    //        stringBuilder.AppendLine($"namespace {symbol.ContainingNamespace};");
//    //        stringBuilder.AppendLine($"partial class {symbol.Name}");
//    //        stringBuilder.AppendLine("{");

//    //        var methods = classDeclarationSyntax.Members
//    //                                            .OfType<MethodDeclarationSyntax>()
//    //                                            .Where(m => m.AttributeLists
//    //                                                         .SelectMany(list => list.Attributes)
//    //                                                         .Any(a => a.Name.ToString().StartsWith(nameof(MapperAttribute).Replace("Attribute", ""))));
//    //        foreach (var method in methods)
//    //        {
//    //            var methodSymbol = model.GetDeclaredSymbol(method);
//    //            if (!methodSymbol.ReturnsVoid && methodSymbol.Parameters.Length is 0 or 1) {
//    //                var firstParameter = methodSymbol.Parameters.FirstOrDefault();
//    //                var sourceType = firstParameter?.Type ?? symbol;
//    //                var sourceParameterName = firstParameter?.Name ?? "this";
//    //                var returnTypeConstructors = methodSymbol.ReturnType.GetMembers()
//    //                                                                    .Where(m => m.Kind == SymbolKind.Method && ((IMethodSymbol)m).MethodKind == MethodKind.Constructor)
//    //                                                                    .Where(m => m.DeclaredAccessibility is Accessibility.Public)
//    //                                                                    .Cast<IMethodSymbol>();
//    //                var hasDefaultConstructor = returnTypeConstructors.Any(c => c.Parameters.Length == 0);

//    //                var parameter = firstParameter != null ? $"{firstParameter.Type} {firstParameter.Name}" : "";
//    //                stringBuilder.AppendLine($"    {methodSymbol.DeclaredAccessibility.ToString().ToLower()} {(methodSymbol.IsStatic ? "static " : "")}partial {methodSymbol.ReturnType} {methodSymbol.Name}({parameter})");
//    //                stringBuilder.AppendLine("    {");
//    //                var targetPublicProperties = methodSymbol.ReturnType.GetMembers().Where(m => m.Kind == SymbolKind.Property).Cast<IPropertySymbol>().Where(p => p.SetMethod is { DeclaredAccessibility: Accessibility.Public }).ToArray();
//    //                var hasPublicProperties = targetPublicProperties.Length != 0;
//    //                if (hasDefaultConstructor)
//    //                {
//    //                    stringBuilder.AppendLine($"        return new {methodSymbol.ReturnType}(){(hasPublicProperties ? "" : ";")}");
//    //                }
//    //                else
//    //                {
//    //                    var constructor = returnTypeConstructors.First();
//    //                    var properties = sourceType.GetAllProperties();
//    //                    var entryValues = constructor.Parameters
//    //                                                 .Select(parameter =>
//    //                                                 {
//    //                                                     var property = properties.FirstOrDefault(prop => prop.Name.Equals(parameter.Name, StringComparison.OrdinalIgnoreCase));
//    //                                                     var nullabilityDivergenceResolver = "";
//    //                                                     if(parameter.NullableAnnotation != NullableAnnotation.Annotated && property.NullableAnnotation == NullableAnnotation.Annotated)
//    //                                                     {
//    //                                                         nullabilityDivergenceResolver = parameter.Type.IsReferenceType ? "!" : ".GetValueOrDefault()";
//    //                                                     }

//    //                                                     return property is not null
//    //                                                             ? $"{sourceParameterName}.{property.Name}{nullabilityDivergenceResolver}"
//    //                                                             : $"default!";
//    //                                                 });
//    //                    stringBuilder.AppendLine($"        return new {methodSymbol.ReturnType}({string.Join(", ", entryValues)}){(hasPublicProperties ? "" : ";")}");
//    //                }
//    //                if (hasPublicProperties)
//    //                {
//    //                    var sourceProperties = sourceType.GetAllProperties();
//    //                    stringBuilder.AppendLine("        {");
//    //                    foreach (var property in targetPublicProperties)
//    //                    {
//    //                        var sourceProperty = sourceProperties.FirstOrDefault(p => p.Name.Equals(property.Name, StringComparison.OrdinalIgnoreCase));
//    //                        if (sourceProperty is null)
//    //                        {
//    //                            continue;
//    //                        }
//    //                        var nullabilityDivergenceResolver = "";
//    //                        if (property.NullableAnnotation != NullableAnnotation.Annotated && sourceProperty.NullableAnnotation == NullableAnnotation.Annotated)
//    //                        {
//    //                            nullabilityDivergenceResolver = property.Type.IsReferenceType ? "!" : ".GetValueOrDefault()";
//    //                        }
//    //                        stringBuilder.AppendLine($"            {property.Name} = {sourceParameterName}.{property.Name}{nullabilityDivergenceResolver},");
//    //                    }
//    //                    stringBuilder.AppendLine("        };");
//    //                }
//    //                stringBuilder.AppendLine("    }");
//    //            }
//    //        }

//    //        stringBuilder.AppendLine("}");

//    //        context.AddSource($"{symbol.Name}.Mapper.g.cs", stringBuilder.ToString());
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
//    //    private static readonly string _mapperAttributeName = nameof(MapperAttribute).Replace("Attribute", "");
//    //    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
//    //    {
//    //        if (syntaxNode is not MethodDeclarationSyntax methodDeclarationSyntax)
//    //        {
//    //            return;
//    //        }

//    //        bool hasMapperAttribute = methodDeclarationSyntax
//    //                                                            .AttributeLists
//    //                                                            .SelectMany(list => list.Attributes)
//    //                                                            .Any(a => a.Name.ToString().StartsWith(_mapperAttributeName));
//    //        if (hasMapperAttribute)
//    //        {
//    //            Classes.Add(methodDeclarationSyntax.Parent as ClassDeclarationSyntax);
//    //        }
//    //    }
//    //}

//}