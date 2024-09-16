using System.Threading.Tasks;
using Xunit;
using Verifier = Microsoft.CodeAnalysis.CSharp.Testing.XUnit.AnalyzerVerifier<Cblx.Blocks.Analyzers.Endpoints.EndpointExecuteAsyncAnalyzer>;

namespace Cblx.Blocks.Analyzers.Endpoints.Tests;

public class EndpointExecuteAsyncAnalyzerTests
{
    [Fact]
    public async Task EndpointShouldContainExecuteAsyncMethod()
    {
        const string text = """
                            namespace GeekSevenLabs
                            {
                                public class SpaceshipOneEndpoint : EndpointBase { }
                                public abstract class EndpointBase { }
                            }
                            """;

        var expected = Verifier.Diagnostic(DiagnosticDescriptors.EndpointShouldContainExecuteAsyncMethod)
            .WithLocation(3, 18)
            .WithArguments("SpaceshipOneEndpoint");
        
        await Verifier.VerifyAnalyzerAsync(text, expected).ConfigureAwait(false);
    }
    
    [Fact]
    public async Task ExecuteAsyncMethodShouldBeStatic()
    {
        const string text = """
                            using System.Threading.Tasks;
                            namespace GeekSevenLabs
                            {
                                public class SpaceshipOneEndpoint : EndpointBase 
                                {
                                    internal Task ExecuteAsync() 
                                    {
                                        return Task.CompletedTask;
                                    }
                                }
                                public abstract class EndpointBase { }
                            }
                            """;

        var expected = Verifier.Diagnostic(DiagnosticDescriptors.ExecuteAsyncMethodShouldBeStatic)
            .WithLocation(6, 23)
            .WithArguments("ExecuteAsync");
        
        await Verifier.VerifyAnalyzerAsync(text, expected).ConfigureAwait(false);
    }
    
    [Fact]
    public async Task ExecuteAsyncMethodShouldBeInternal()
    {
        const string text = """
                            using System.Threading.Tasks;
                            namespace GeekSevenLabs
                            {
                                public class SpaceshipOneEndpoint : EndpointBase 
                                {
                                    public static Task ExecuteAsync() 
                                    {
                                        return Task.CompletedTask;
                                    }
                                }
                                public abstract class EndpointBase { }
                            }
                            """;

        var expected = Verifier.Diagnostic(DiagnosticDescriptors.ExecuteAsyncMethodShouldBeInternal)
            .WithLocation(6, 28)
            .WithArguments("ExecuteAsync");
        
        await Verifier.VerifyAnalyzerAsync(text, expected).ConfigureAwait(false);
    }
    
}