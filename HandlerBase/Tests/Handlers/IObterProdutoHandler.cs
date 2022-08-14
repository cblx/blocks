namespace Cblx.Blocks.HandlerBase.Tests.Handlers;

[GenerateClient(RoutePrefix = "prefix")]
public interface IObterProdutoHandler
{
    public Task<ObterProdutoResponse> GetAsync(ObterProdutoRequest request);
}
