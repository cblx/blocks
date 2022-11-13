﻿using Microsoft.CodeAnalysis;

namespace Cblx.Blocks.SourceGenerators.DependencyInjection;

[Generator]
public class AssemblyMarkerGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        string assemblyName = context.Compilation.AssemblyName;
        
        string markerName = $"{assemblyName.Replace(".", "")}Marker";
#pragma warning disable S2479 // Whitespace and control characters in string literals should be explicit
        string source = $$""" 
            // Auto-generated code
            using System.Reflection;
            using System.Diagnostics.CodeAnalysis;
            namespace {{assemblyName}};
            [ExcludeFromCodeCoverage]
            public static class {{markerName}}
            { 
                public static Assembly Assembly = typeof({{markerName}}).Assembly; 
            }
            """;
#pragma warning restore S2479 // Whitespace and control characters in string literals should be explicit
        context.AddSource("Marker.g.cs", source);
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        // Nada necessário durante inicialização deste Gerador
    }
}