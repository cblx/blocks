using System.Threading.Tasks;
using Xunit;
using Verifier = Microsoft.CodeAnalysis.CSharp.Testing.XUnit.CodeFixVerifier<
    Cblx.Blocks.Analyzers.Endpoints.EndpointExecuteAsyncAnalyzer,
    Cblx.Blocks.Analyzers.Endpoints.EndpointInternalExecuteAsyncProvider>;

namespace Cblx.Blocks.Analyzers.Endpoints.Tests;

public class EndpointInternalExecuteAsyncProviderTests
{
    [Fact]
    public async Task MethodWithoutInternalKeyword_AddInternalKeyword()
    {
        const string text = """
                            using System.Threading.Tasks;
                            namespace GeekSevenLabs
                            {
                                public class SpaceshipOneEndpointWithoutInternal : EndpointBase 
                                {
                                    public static async Task ExecuteAsync() 
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
                                   public class SpaceshipOneEndpointWithoutInternal : EndpointBase 
                                   {
                                       internal static async Task ExecuteAsync() 
                                       {
                                           await Task.CompletedTask;
                                       }
                                   }
                                   public abstract class EndpointBase { }
                               }
                               """;

        var expected = Verifier.Diagnostic(DiagnosticDescriptors.ExecuteAsyncMethodShouldBeInternal)
            .WithLocation(6, 34)
            .WithArguments("ExecuteAsync");

        await Verifier.VerifyCodeFixAsync(text, expected, newText).ConfigureAwait(false);
    }
}