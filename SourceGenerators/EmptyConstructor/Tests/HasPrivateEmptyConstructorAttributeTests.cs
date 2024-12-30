using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Cblx.Blocks.SourceGenerators.EmptyConstructor.Tests;
using VerifyCS = CSharpSourceGeneratorVerifier<EmptyConstructorGenerator>;
public class HasPrivateEmptyConstructorAttributeTests
{
    [Fact]
    public async Task DeveGerarUmConstrutorPrivadoSemParametros()
    {
        var code = """
            using Cblx.Blocks;
            namespace MyNamespace;
            [HasPrivateEmptyConstructor]
            public partial class MyClass {}
            """;
        var generated = """
            using System.Diagnostics.CodeAnalysis;
                    
            namespace MyNamespace;
            partial class MyClass
            {
            #pragma warning disable CS8618
                [ExcludeFromCodeCoverage]
                private MyClass(){}
            #pragma warning restore CS8618
            }
            """;
        await new VerifyCS.Test
        {
            TestState =
            {
                Sources = { code },
                GeneratedSources =
                {
                    (typeof(EmptyConstructorGenerator), "MyClass.g.cs", SourceText.From(generated, Encoding.UTF8, SourceHashAlgorithm.Sha1)),
                },
                AdditionalReferences =
                {
                    MetadataReference.CreateFromFile(typeof(HasPrivateEmptyConstructorAttribute).Assembly.Location),
                }
            },
        }.RunAsync();
    }

    [Fact]
    public async Task DeveGerarUmConstrutorPrivadoSemParametrosUsandoDefaultConstructor()
    {
        var code = """
            using Cblx.Blocks;
            namespace MyNamespace;
            [HasPrivateEmptyConstructor]
            public partial class MyClass(int a, string b, string c) {}
            """;
        var generated = """
            using System.Diagnostics.CodeAnalysis;
                    
            namespace MyNamespace;
            partial class MyClass
            {
            #pragma warning disable CS8618
                [ExcludeFromCodeCoverage]
                private MyClass() : this(default!, default!, default!) {}
            #pragma warning restore CS8618
            }
            """;
        await new VerifyCS.Test
        {
            TestState =
            {
                Sources = { code },
                GeneratedSources =
                {
                    (typeof(EmptyConstructorGenerator), "MyClass.g.cs", SourceText.From(generated, Encoding.UTF8, SourceHashAlgorithm.Sha1)),
                },
                AdditionalReferences =
                {
                    MetadataReference.CreateFromFile(typeof(HasPrivateEmptyConstructorAttribute).Assembly.Location),
                }
            },
        }.RunAsync();
    }


    [Fact]
    public void NaoDeveConseguirCriarUmaInstancia()
    {
        var exec = () => Activator.CreateInstance<MyClass>();
        exec.Should()
            .Throw<MissingMethodException>()
            .WithMessage("No parameterless constructor defined for type 'Cblx.Blocks.SourceGenerators.EmptyConstructor.Tests.MyClass'.");
    }

    [Fact]
    public void DeveConseguirCriarUmaInstanciaComUmConstrutorPrivado()
    {
        var exec = () => Activator.CreateInstance(typeof(MyClass), true);
        exec.Should().NotThrow();
    }

    [Fact]
    public void DeveConseguirCriarUmaInstanciaComUmConstrutorPrivadoUsandoPrimaryConstructor()
    {
        var exec = () => Activator.CreateInstance(typeof(MyClassPrimary), true);
        exec.Should().NotThrow();
    }

}


[HasPrivateEmptyConstructor]
public partial class MyClass { }

[HasPrivateEmptyConstructor]
public partial class MyClassPrimary(string a, int b, DateTime c) { }
