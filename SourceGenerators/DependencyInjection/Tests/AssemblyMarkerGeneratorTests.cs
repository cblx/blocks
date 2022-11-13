using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace Cblx.Blocks.SourceGenerators.DependencyInjection.Tests;
using VerifyCS = CSharpSourceGeneratorVerifier<AssemblyMarkerGenerator>;
public class AssemblyMarkerGeneratorTests
{
    [Fact]
    public async Task ShouldGenerateAnAssemblyMarkerForTheCurrentAssembly()
    {
        var code = "// does not matter, will be generated a class for the assembly";
        var generated = """
            // Auto-generated code
            using System.Reflection;
            using System.Diagnostics.CodeAnalysis;
            namespace TestProject;
            [ExcludeFromCodeCoverage]
            public static class TestProjectMarker
            { 
                public static Assembly Assembly = typeof(TestProjectMarker).Assembly; 
            }
            """;
        await new VerifyCS.Test
        {
            TestState =
            {
                Sources = { code },
                GeneratedSources =
                {
                    (typeof(AssemblyMarkerGenerator), "Marker.g.cs", SourceText.From(generated, Encoding.UTF8, SourceHashAlgorithm.Sha1)),
                }
            },
        }.RunAsync();
    }
}