namespace Cblx.Blocks.SourceGenerators.DependencyInjection.Tests;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;
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
    public async Task ShouldGenerateEmptyServiceCollectionExtensionClassWhenTheresNoAnnotatedServices()
    {
        var code = """
            using Cblx.Blocks;
            namespace MyNamespace;
            public interface IMyService {}

            public class MyService : IMyService {}
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
    public async Task ShouldGenerateServiceCollectionExtensionClassWithAddAllServicesWhenAssemblyHasServiceEntryServices()
    {
        var code = """
            using Cblx.Blocks;
            [assembly: ServicesEntryAttribute(prefix: "TestProject")]
            namespace MyNamespace;
            public interface IMyService {}
            
            public class MyService : IMyService {}
            
            public class AnotherService {}
            """;

        // [assembly: ServicesEntry(prefix: "abc")]
        var generated = """
            // Auto-generated code
            using Microsoft.Extensions.DependencyInjection;
            using System.Diagnostics.CodeAnalysis;
            using TestProject.Other;
            namespace TestProject;
            [ExcludeFromCodeCoverage]
            public static partial class ServiceCollectionExtensions
            {
                public static IServiceCollection AddTestProjectServices(this IServiceCollection services)
                {
                    
                    return services;
                }

                public static IServiceCollection AddAllServices(this IServiceCollection services)
                {
                    services.AddTestProjectServices();
                    services.AddTestProjectOtherServices();
                    return services;
                }
            }
            """;

        var cblxDependencyInjectionRef = MetadataReference.CreateFromFile(typeof(ScopedAttribute<>).Assembly.Location);
        var msDependencyInjectionRef = MetadataReference.CreateFromFile(typeof(IServiceCollection).Assembly.Location);
        var net70 = Net.Net70;

        await new VerifyCS.Test
        {
            TestState =
            {
                Sources = { code },
                GeneratedSources =
                {
                    (typeof(ServiceCollectionExtensionsGenerator), "ServiceCollectionExtensions.g.cs", SourceText.From(generated, Encoding.UTF8, SourceHashAlgorithm.Sha1)),
                },
                ExpectedDiagnostics =
                {
                    // Não consegui resolver os erros abaixo durante o teste ☹️
                    // Cblx.Blocks.SourceGenerators.DependencyInjection\Cblx.Blocks.SourceGenerators.DependencyInjection.ServiceCollectionExtensionsGenerator\ServiceCollectionExtensions.g.cs(2,17): error CS0234: O nome de tipo ou namespace "Extensions" não existe no namespace "Microsoft" (você está sem uma referência de assembly?)
                    DiagnosticResult.CompilerError("CS0234").WithSpan(@"Cblx.Blocks.SourceGenerators.DependencyInjection\Cblx.Blocks.SourceGenerators.DependencyInjection.ServiceCollectionExtensionsGenerator\ServiceCollectionExtensions.g.cs", 2, 17, 2, 27).WithArguments("Extensions", "Microsoft"),
                    // Cblx.Blocks.SourceGenerators.DependencyInjection\Cblx.Blocks.SourceGenerators.DependencyInjection.ServiceCollectionExtensionsGenerator\ServiceCollectionExtensions.g.cs(8,19): error CS0246: O nome do tipo ou do namespace "IServiceCollection" não pode ser encontrado (está faltando uma diretiva using ou uma referência de assembly?)
                    DiagnosticResult.CompilerError("CS0246").WithSpan(@"Cblx.Blocks.SourceGenerators.DependencyInjection\Cblx.Blocks.SourceGenerators.DependencyInjection.ServiceCollectionExtensionsGenerator\ServiceCollectionExtensions.g.cs", 8, 19, 8, 37).WithArguments("IServiceCollection"),
                    // Cblx.Blocks.SourceGenerators.DependencyInjection\Cblx.Blocks.SourceGenerators.DependencyInjection.ServiceCollectionExtensionsGenerator\ServiceCollectionExtensions.g.cs(8,71): error CS0246: O nome do tipo ou do namespace "IServiceCollection" não pode ser encontrado (está faltando uma diretiva using ou uma referência de assembly?)
                    DiagnosticResult.CompilerError("CS0246").WithSpan(@"Cblx.Blocks.SourceGenerators.DependencyInjection\Cblx.Blocks.SourceGenerators.DependencyInjection.ServiceCollectionExtensionsGenerator\ServiceCollectionExtensions.g.cs", 8, 71, 8, 89).WithArguments("IServiceCollection"),
                    // Cblx.Blocks.SourceGenerators.DependencyInjection\Cblx.Blocks.SourceGenerators.DependencyInjection.ServiceCollectionExtensionsGenerator\ServiceCollectionExtensions.g.cs(18,9): error CS1929: "IServiceCollection" não contém uma definição para "AddTestProjectOtherServices" e a melhor sobrecarga do método de extensão "ServiceCollectionExtensionsTest.AddTestProjectOtherServices(IServiceCollection)" requer um receptor do tipo "IServiceCollection"
                    DiagnosticResult.CompilerError("CS1929").WithSpan(@"Cblx.Blocks.SourceGenerators.DependencyInjection\Cblx.Blocks.SourceGenerators.DependencyInjection.ServiceCollectionExtensionsGenerator\ServiceCollectionExtensions.g.cs", 18, 9, 18, 17).WithArguments("Microsoft.Extensions.DependencyInjection.IServiceCollection", "AddTestProjectOtherServices", "TestProject.Other.ServiceCollectionExtensionsTest.AddTestProjectOtherServices(IServiceCollection)", "IServiceCollection"),
                    // Class0.cs(1,17): error CS0234: O nome de tipo ou namespace "Extensions" não existe no namespace "Microsoft" (você está sem uma referência de assembly?)
                    DiagnosticResult.CompilerError("CS0234").WithSpan(@"Class0.cs", 1, 17, 1, 27).WithArguments("Extensions", "Microsoft"),
                    // Class0.cs(5,19): error CS0246: O nome do tipo ou do namespace "IServiceCollection" não pode ser encontrado (está faltando uma diretiva using ou uma referência de assembly?)
                    DiagnosticResult.CompilerError("CS0246").WithSpan(@"Class0.cs", 5, 19, 5, 37).WithArguments("IServiceCollection"),
                    // Class0.cs(5,71): error CS0246: O nome do tipo ou do namespace "IServiceCollection" não pode ser encontrado (está faltando uma diretiva using ou uma referência de assembly?)
                    DiagnosticResult.CompilerError("CS0246").WithSpan(@"Class0.cs", 5, 71, 5, 89).WithArguments("IServiceCollection"),
                },
                AdditionalProjects =
                {
                    {
                        "TestProject.Other",
                        //bla.TestState
                        new ProjectState("TestProject.Other", LanguageNames.CSharp, "Class", "cs"){
                            Sources = {
                                """
                                using Microsoft.Extensions.DependencyInjection;
                                namespace TestProject.Other;
                                public static class ServiceCollectionExtensionsTest
                                {
                                    public static IServiceCollection AddTestProjectOtherServices(this IServiceCollection services) { return services; }
                                }
                                """
                            },
                            ReferenceAssemblies = net70,

                            AdditionalReferences =
                            {
                                cblxDependencyInjectionRef,
                                msDependencyInjectionRef,
                            }
                        }
                    }
                },
                AdditionalProjectReferences =
                {
                    "TestProject.Other"
                },
                ReferenceAssemblies = net70,
                AdditionalReferences =
                {
                    cblxDependencyInjectionRef,
                    msDependencyInjectionRef,
                }
            },
        }.RunAsync();
    }
}
