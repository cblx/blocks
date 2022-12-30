using System.Text;
using Cblx.Blocks.Models;

namespace Cblx.Blocks.Templates;

internal static class ServiceCollectionTemplate
{
    public static string CreateAddScopedLine(HandlerDeclaration handler)
    {
        var namespaceBase = handler.HandlerNamespace;
        var contract = handler.InterfaceName;
        var service = handler.ImplementationName;

        return $"        services.AddScoped<{namespaceBase}.{contract}, {namespaceBase}.{service}Client>();";
    }

    public static string Create(StringBuilder stringBuilder, string assemblyName, string addServicesName)
    {
        return$$"""
            // Auto-generated code
            using Microsoft.Extensions.DependencyInjection;
            using System.Diagnostics.CodeAnalysis;

            namespace {{assemblyName}};

            [ExcludeFromCodeCoverage]
            public static partial class ServiceCollectionExtensionsForClientHandlers
            {
                public static IServiceCollection {{addServicesName}}(this IServiceCollection services)
                {
            {{stringBuilder}}
                    return services;
                }
            }
            """;
    }
}