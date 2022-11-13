namespace Cblx.Blocks.SourceGenerators.DependencyInjection.Tests;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using VerifyCS = CSharpSourceGeneratorVerifier<ServiceCollectionExtensionsGenerator>;
public class ServiceCollectionExtensionsGeneratorTests
{
    [Fact]
    public async Task ShouldGenerateServiceCollectionExtensionClassWithAddServices()
    {
        var code = """
            using Cblx.Blocks;
            namespace MyNamespace;
            public interface IMyService {}

            [Scoped<IMyService>]
            public class MyService : IMyService {}

            [Scoped]
            public class AnotherService {}
            """;
        var generated = """
            // Auto-generated code
            using Microsoft.Extensions.DependencyInjection;
            using System.Diagnostics.CodeAnalysis;
            namespace TestProject;
            [ExcludeFromCodeCoverage]
            public static partial class ServiceCollectionExtensions
            {
                public static IServiceCollection AddTestProjectServices(this IServiceCollection services)
                {
                    services.AddScoped<MyNamespace.IMyService, MyNamespace.MyService>();
                    services.AddScoped<MyNamespace.AnotherService>();
                    return services;
                }
            }
            """;
        await new VerifyCS.Test
        {
            TestState =
            {
                Sources = { code },
                GeneratedSources =
                {
                    (typeof(ServiceCollectionExtensionsGenerator), "ServiceCollectionExtensions.g.cs", SourceText.From(generated, Encoding.UTF8, SourceHashAlgorithm.Sha1)),
                },
                ReferenceAssemblies = Net.Net70,
                AdditionalReferences =
                {
                    MetadataReference.CreateFromFile(typeof(ScopedAttribute<>).Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(IServiceCollection).Assembly.Location),
                }
            },
        }.RunAsync();
    }

    [Fact]
    public async Task ShouldNotGenerateServiceCollectionExtensionClassWhenTheresNoAnnotatedServices()
    {
        var code = """
            using Cblx.Blocks;
            namespace MyNamespace;
            public interface IMyService {}

            public class MyService : IMyService {}
            """;

        await new VerifyCS.Test
        {
            TestState =
            {
                Sources = { code },
                GeneratedSources = {},
                ReferenceAssemblies = Net.Net70,
                AdditionalReferences =
                {
                    MetadataReference.CreateFromFile(typeof(ScopedAttribute<>).Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(IServiceCollection).Assembly.Location),
                }
            },
        }.RunAsync();
    }
}
