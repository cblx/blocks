namespace Cblx.Blocks.Analyzers.Endpoints;

internal sealed class EndpointExecuteAsyncOptions
{
    private const string DotnetAnalyzerConfig = "dotnet_analyzer_config";
    private const string CblxAnalyzerEndpoints = "cblx_analyzer_endpoints";
    
    public const string DeclaredAccessibilityAnalyzerName = $"{DotnetAnalyzerConfig}.{CblxAnalyzerEndpoints}.use_declared_accessibility_analyzer";

    public bool UseDeclaredAccessibilityAnalyzer { get; set; } = true;
}