using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cblx.Blocks.SourceGenerators.Dto.Tests;
internal static class TestHelpers
{
    public static IEnumerable<MetadataReference> CommonReferences(params Assembly[] additionalRereferences)
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
            .Select(_ => MetadataReference.CreateFromFile(_.Location))
            .Concat(additionalRereferences.Select(_ => MetadataReference.CreateFromFile(_.Location)));
    }

    public static CSharpCompilation CreateCompilation(string code, params Assembly[] additionalRereferences)
    {
        return CSharpCompilation.Create("TestProject", [CSharpSyntaxTree.ParseText(code)], CommonReferences(additionalRereferences), new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
    }

    public static void AssertGeneration<TGenerator>(
        string inputCode, 
        string expectedOutputCode,
        string step,
        params Assembly[] additionalReferences)
        where TGenerator: IIncrementalGenerator, new()
    {
        var generator = new TGenerator();
        var compilation = CreateCompilation(inputCode, additionalReferences);
        GeneratorDriver driver = CSharpGeneratorDriver.Create([generator.AsSourceGenerator()], driverOptions: new GeneratorDriverOptions(default, trackIncrementalGeneratorSteps: true));
        driver = driver.RunGenerators(compilation);
        // Update the compilation and rerun the generator
        compilation = compilation.AddSyntaxTrees(CSharpSyntaxTree.ParseText("// dummy"));
        driver = driver.RunGenerators(compilation);
        // Assert the driver doesn't recompute the output
        var result = driver.GetRunResult().Results.Single();
        var allOutputs = result.TrackedOutputSteps.SelectMany(outputStep => outputStep.Value).SelectMany(output => output.Outputs);
        Assert.Collection(allOutputs, output => Assert.Equal(IncrementalStepRunReason.Cached, output.Reason));
        // Assert the driver use the cached result from Extraction
        var extractionOutputs = result.TrackedSteps[step].Single().Outputs;
        Assert.Collection(extractionOutputs, output => Assert.Equal(IncrementalStepRunReason.Unchanged, output.Reason));
        result.GeneratedSources[0].SourceText.ToString().Should().Be(expectedOutputCode);
    }
}
