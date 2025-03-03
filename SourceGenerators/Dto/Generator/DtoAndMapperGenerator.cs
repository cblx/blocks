using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;

namespace Cblx.Blocks.SourceGenerators.Dto;
[Generator]
public class DtoAndMapperGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var dtoPipeline = CreateDtoPipeline(context);
        var mapperPipeline = CreateMapperPipeline(context);
        context.RegisterSourceOutput(dtoPipeline, ProduceDtoSource);
        context.RegisterSourceOutput(mapperPipeline, ProduceMapperSource);
    }

    private static void ProduceMapperSource(SourceProductionContext context, MapperExtractionModel model)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("#nullable enable");
        stringBuilder.AppendLine($"namespace {model.Namespace};");
        stringBuilder.AppendLine($"partial class {model.ClassName}");
        stringBuilder.AppendLine("{");
        WriteMapperMethod(stringBuilder, model);
        // var parameter = model.SourceName != "this" ? $"{model.SourceType} {model.SourceName}" : "";
        //stringBuilder.AppendLine($"    {model.MethodModifiers} partial {model.ReturnTypeName} {model.MethodName}({parameter})");
        //stringBuilder.AppendLine("    {");
        //var constructorParameters = new List<string>();
        //foreach (var p in model.ConstructorParameters)
        //{
        //    var sourceProperty = model.SourceProperties.FirstOrDefault(sp => sp.Name.Equals(p.Name, StringComparison.OrdinalIgnoreCase));
        //    if (sourceProperty is null)
        //    {
        //        constructorParameters.Add($"default!");
        //        continue;
        //    }
        //    var nullabilityDivergenceResolver = "";
        //    if (!p.Nullable && sourceProperty.Nullable)
        //    {
        //        nullabilityDivergenceResolver = p.IsReferenceType ? "!" : ".GetValueOrDefault()";
        //    }
        //    constructorParameters.Add($"{model.SourceName}.{sourceProperty.Name}{nullabilityDivergenceResolver}");
        //}
        //stringBuilder.AppendLine($"        return new {model.ReturnTypeName}({string.Join(", ", constructorParameters)}){(model.TargetProperties.Any() ? "" : ";")}");
        //if (model.TargetProperties.Any())
        //{
        //    stringBuilder.AppendLine("        {");
        //    foreach (var property in model.TargetProperties)
        //    {
        //        var sourceProperty = model.SourceProperties.FirstOrDefault(p => p.Name.Equals(property.Name, StringComparison.OrdinalIgnoreCase));
        //        if (sourceProperty is null)
        //        {
        //            continue;
        //        }
        //        var nullabilityDivergenceResolver = "";
        //        if (!property.Nullable && sourceProperty.Nullable)
        //        {
        //            nullabilityDivergenceResolver = property.IsReferenceType ? "!" : ".GetValueOrDefault()";
        //        }
        //        stringBuilder.AppendLine($"            {property.Name} = {model.SourceName}.{property.Name}{nullabilityDivergenceResolver},");
        //    }
        //    stringBuilder.AppendLine("        };");
        //}
        //stringBuilder.AppendLine("    }");
        stringBuilder.AppendLine("}");
        context.AddSource($"{model.ClassName}.{model.MethodName}.Mapper.g.cs", SourceText.From(stringBuilder.ToString(), Encoding.UTF8));

    }

    private static void WriteMapperMethod(StringBuilder stringBuilder, MapperExtractionModel model)
    {
        var parameter = model.SourceName != "this" ? $"{model.SourceType} {model.SourceName}" : "";
        stringBuilder.AppendLine($"    {model.MethodModifiers} partial {model.ReturnTypeName} {model.MethodName}({parameter})");
        stringBuilder.AppendLine("    {");
        var constructorParameters = new List<string>();
        foreach (var p in model.ConstructorParameters)
        {
            var sourceProperty = model.SourceProperties.FirstOrDefault(sp => sp.Name.Equals(p.Name, StringComparison.OrdinalIgnoreCase));
            if (sourceProperty is null)
            {
                constructorParameters.Add($"default!");
                continue;
            }
            var nullabilityDivergenceResolver = "";
            if (!p.Nullable && sourceProperty.Nullable)
            {
                nullabilityDivergenceResolver = p.IsReferenceType ? "!" : ".GetValueOrDefault()";
            }
            constructorParameters.Add($"{model.SourceName}.{sourceProperty.Name}{nullabilityDivergenceResolver}");
        }
        var combiningProperties = (from tp in model.TargetProperties
                                   join sp in model.SourceProperties on tp.Name equals sp.Name
                                   select new { Target = tp, Source = sp }).ToArray();
        stringBuilder.AppendLine($"        return new {model.ReturnTypeName}({string.Join(", ", constructorParameters)}){(combiningProperties.Any() ? "" : ";")}");
        if (combiningProperties.Any())
        {
            stringBuilder.AppendLine("        {");
            foreach (var combo in combiningProperties)
            {
                var nullabilityDivergenceResolver = "";
                if (!combo.Target.Nullable && combo.Source.Nullable)
                {
                    nullabilityDivergenceResolver = combo.Target.IsReferenceType ? "!" : ".GetValueOrDefault()";
                }
                stringBuilder.AppendLine($"            {combo.Target.Name} = {model.SourceName}.{combo.Source.Name}{nullabilityDivergenceResolver},");
            }
            stringBuilder.AppendLine("        };");
        }
        stringBuilder.AppendLine("    }");
    }

    private static IncrementalValuesProvider<MapperExtractionModel> CreateMapperPipeline(IncrementalGeneratorInitializationContext context)
    {
        var mapperPipeline = context.SyntaxProvider.ForAttributeWithMetadataName(
           fullyQualifiedMetadataName: "Cblx.Blocks.MapperAttribute",
           predicate: static (syntaxNode, cancellationToken) => syntaxNode is BaseMethodDeclarationSyntax,
           // When returning a value type or record, or a custom comparer, the generation can be optimized.
           // more: https://andrewlock.net/creating-a-source-generator-part-9-avoiding-performance-pitfalls-in-incremental-generators/#3-use-a-value-type-data-model-or-records-or-a-custom-comparer-
           transform: static (context, cancellationToken) =>
           {
               var methodSymbol = (context.TargetSymbol as IMethodSymbol)!;
               // if is inside a [DtoOf<...>] annotated class, ignore
               if (methodSymbol.ContainingType.GetAttributes().Any(a => a.AttributeClass?.Name == nameof(DtoOfAttribute<object>))) { return null; }
               return CreateMapperModel(methodSymbol);
           }
       ).WithTrackingName("MapperExtraction").Where(m => m != null);
        return mapperPipeline!;
    }

    private static MapperExtractionModel? CreateMapperModel(
        IMethodSymbol methodSymbol,
        MapperContainingDto? containingDto = null)
    {
        var containingType = methodSymbol.ContainingType;
        var firstParameter = methodSymbol.Parameters.FirstOrDefault();
        var sourceType = firstParameter?.Type ?? containingType;
        var targetType = methodSymbol.ReturnType;
        var targetConstructor = targetType.GetMembers()
                                          .Where(m => m is IMethodSymbol { MethodKind: MethodKind.Constructor, DeclaredAccessibility: Accessibility.Public })
                                          .Cast<IMethodSymbol>()
                                          .OrderBy(m => m.Parameters.Length)
                                          .FirstOrDefault();
        var containingDtoIsSource = containingDto?.DtoType.Equals(sourceType, SymbolEqualityComparer.Default) ?? false;
        var containingDtoIsTarget = containingDto?.DtoType.Equals(targetType, SymbolEqualityComparer.Default) ?? false;

        var shouldGenerate = !methodSymbol.ReturnsVoid && methodSymbol.Parameters.Length is 0 or 1 && targetConstructor != null;
        if (!shouldGenerate) { return null; }
        var targetProperties = targetType.GetAllProperties().Where(p => p.SetMethod is { DeclaredAccessibility: Accessibility.Public }).ToArray();
        var sourceProperties = sourceType.GetAllProperties().Where(p => p.GetMethod is { DeclaredAccessibility: Accessibility.Public }).ToArray();

        var constructorParameters = targetConstructor?.Parameters ?? [];

        var model = new MapperExtractionModel
        {
            // Note: this is a simplified example. You will also need to handle the case where the type is in a global namespace, nested, etc.
            Namespace = containingType.ContainingNamespace?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat.WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle.Omitted)) ?? "",
            ClassName = containingType.Name,
            MethodName = methodSymbol.Name,
            ReturnTypeName = methodSymbol.ReturnType.ToString(),
            MethodModifiers = $"{methodSymbol.DeclaredAccessibility.ToString().ToLower()}{(methodSymbol.IsStatic ? " static" : "")}",
            SourceName = firstParameter?.Name ?? "this",
            SourceType = sourceType.ToString(),
            ConstructorParameters = new EquatableArray<Argument>(constructorParameters.Select(p => new Argument
            {
                Name = p.Name,
                Nullable = p.NullableAnnotation == NullableAnnotation.Annotated,
                IsReferenceType = p.Type.IsReferenceType
            }).ToArray()),
            SourceProperties = new EquatableArray<Argument>(sourceProperties.Select(p => new Argument
            {
                Name = p.Name,
                Nullable = p.NullableAnnotation == NullableAnnotation.Annotated,
                IsReferenceType = p.Type.IsReferenceType
            }).Union(containingDtoIsSource ? containingDto!.GeneratedProperties : []).ToArray()),
            TargetProperties = new EquatableArray<Argument>(targetProperties.Select(p => new Argument
            {
                Name = p.Name,
                Nullable = p.NullableAnnotation == NullableAnnotation.Annotated,
                IsReferenceType = p.Type.IsReferenceType
            }).Union(containingDtoIsTarget ? containingDto!.GeneratedProperties : []).ToArray())
        };
        return model;
    }

    private static void ProduceDtoSource(SourceProductionContext context, DtoExtractionModel model)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("#nullable enable");
        stringBuilder.AppendLine($"namespace {model.Namespace};");
        stringBuilder.AppendLine($"partial class {model.ClassName}");
        stringBuilder.AppendLine("{");
        foreach (var property in model.Properties)
        {
            stringBuilder.AppendLine(property);
        }
        foreach (var mapper in model.Mappers)
        {
            WriteMapperMethod(stringBuilder, mapper);
        }
        stringBuilder.AppendLine("}");

        context.AddSource($"{model.ClassName}.Dto.g.cs", SourceText.From(stringBuilder.ToString(), Encoding.UTF8));
    }

    private static IncrementalValuesProvider<DtoExtractionModel> CreateDtoPipeline(IncrementalGeneratorInitializationContext context)
    {
        var dtoPipeline = context.SyntaxProvider.ForAttributeWithMetadataName(
           fullyQualifiedMetadataName: "Cblx.Blocks.DtoOfAttribute`1",
           predicate: static (syntaxNode, cancellationToken) => syntaxNode is ClassDeclarationSyntax,
           // When returning a value type or record, or a custom comparer, the generation can be optimized.
           // more: https://andrewlock.net/creating-a-source-generator-part-9-avoiding-performance-pitfalls-in-incremental-generators/#3-use-a-value-type-data-model-or-records-or-a-custom-comparer-
           transform: static (context, cancellationToken) =>
           {
               var sourceClass = context.Attributes[0].AttributeClass!.TypeArguments[0];
               var annotatedClass = (context.TargetSymbol as INamedTypeSymbol)!;
               var ignoredProps = annotatedClass.GetAttributes()
                   .Where(a => new string[] { nameof(DtoIgnoreAttribute) }.Contains(a.AttributeClass?.Name))
                   .SelectMany(a => a.NamedArguments.Select(c => c.Value.Value?.ToString()))
                   .ToArray();
               var properties = sourceClass.GetAllProperties().Where(property => !ignoredProps.Contains(property.Name)).Select(property =>
               {
                   var typeAsNullable = property.Type.IsReferenceType ? $"{property.Type.WithNullableAnnotation(NullableAnnotation.Annotated)}"
                                                                          : property.Type.Name != "Nullable" ? $"{property.Type}?" : $"{property.Type}";
                   return $"    public {typeAsNullable} {property.Name} {{ get; set; }}";
               }).ToArray();
               var generatedProperties = sourceClass.GetAllProperties().Where(property => !ignoredProps.Contains(property.Name)).Select(property =>
               {
                   return new Argument
                   {
                       Name = property.Name,
                       Nullable = true,
                       IsReferenceType = property.Type.IsReferenceType
                   };
               }).ToArray();

               var mapperMethodSymbols = annotatedClass.GetMembers()
                             .Where(m => m.Kind == SymbolKind.Method).Cast<IMethodSymbol>()
                             .Where(m => m.GetAttributes().Any(a => a.AttributeClass?.Name == nameof(MapperAttribute)))
                             .ToArray();

               var mapperContainingDto = new MapperContainingDto
               {
                   DtoType = annotatedClass,
                   GeneratedProperties = generatedProperties
               };
               return new DtoExtractionModel
               {
                   // Note: this is a simplified example. You will also need to handle the case where the type is in a global namespace, nested, etc.
                   Namespace = annotatedClass.ContainingNamespace?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat.WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle.Omitted)) ?? "",
                   ClassName = annotatedClass.Name,
                   Properties = new EquatableArray<string>(properties),
                   Mappers = new EquatableArray<MapperExtractionModel>(mapperMethodSymbols.Select(m => CreateMapperModel(m, mapperContainingDto))
                                                                                          .Where(m => m != null).ToArray()!)
               };
           }
       ).WithTrackingName("DtoExtraction");
        return dtoPipeline;
    }

    class MapperContainingDto
    {
        public INamedTypeSymbol DtoType { get; set; } = default!;
        public IEnumerable<Argument> GeneratedProperties { get; set; } = default!;
    }
}