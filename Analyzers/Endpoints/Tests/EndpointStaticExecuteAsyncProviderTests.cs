using System.Threading.Tasks;
using Xunit;
using Verifier = Microsoft.CodeAnalysis.CSharp.Testing.XUnit.CodeFixVerifier<
    Cblx.Blocks.Analyzers.Endpoints.EndpointExecuteAsyncAnalyzer,
    Cblx.Blocks.Analyzers.Endpoints.EndpointStaticExecuteAsyncProvider>;

namespace Cblx.Blocks.Analyzers.Endpoints.Tests;

public class EndpointStaticExecuteAsyncProviderTests
{
    [Fact]
    public async Task MethodWithoutStaticKeyword_AddStaticKeyword()
    {
        const string text = """
                            using System.Threading.Tasks;
                            namespace GeekSevenLabs
                            {
                                public class SpaceshipOneEndpoint : EndpointBase 
                                {
                                    internal async Task ExecuteAsync() 
                                    {
                                        await Task.CompletedTask;
                                    }
                                }
                                public abstract class EndpointBase { }
                            }
                            """;

        const string newText = """
                               using System.Threading.Tasks;
                               namespace GeekSevenLabs
                               {
                                   public class SpaceshipOneEndpoint : EndpointBase 
                                   {
                                       internal static async Task ExecuteAsync() 
                                       {
                                           await Task.CompletedTask;
                                       }
                                   }
                                   public abstract class EndpointBase { }
                               }
                               """;

        var expected = Verifier.Diagnostic(DiagnosticDescriptors.ExecuteAsyncMethodShouldBeStatic)
            .WithLocation(6,29)
            .WithArguments("ExecuteAsync");

        await Verifier.VerifyCodeFixAsync(text, expected, newText).ConfigureAwait(false);

    }
}