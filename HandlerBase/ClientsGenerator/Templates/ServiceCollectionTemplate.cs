using System;
using System.Collections.Generic;
using System.Linq;
using Cblx.Blocks.Models;

namespace Cblx.Blocks.Templates;

internal static class ServiceCollectionTemplate
{
    private static readonly IList<string> Services = new List<string>();

    public static void Clean() => Services.Clear(); 

    public static void AddScoped(HandlerDeclaration handler)
    {
        var namespaceBase = handler.HandlerNamespace;
        var contract = handler.InterfaceName;
        var service = handler.ImplementationName;
        
        Services.Add($"services.AddScoped<{namespaceBase}.{contract}, {namespaceBase}.{service}Client>();");
    }

    public static string? CreateOrDefault(string assemblyName, string addServicesName)
    {
        return Services.Any() is false ? null : $$"""
            // Auto-generated code
            using Microsoft.Extensions.DependencyInjection;
            using System.Diagnostics.CodeAnalysis;

            namespace {{assemblyName}};

            [ExcludeFromCodeCoverage]
            public static partial class ServiceCollectionExtensionsForClientHandlers
            {
                public static IServiceCollection {{addServicesName}}(this IServiceCollection services)
                {
                    {{ToAddServices()}}
                    return services;
                }
            }
            """;
    }

    private static string ToAddServices() => string.Join($"{Environment.NewLine}        ", Services);
}