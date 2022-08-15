namespace Cblx.Blocks.HandlerBase.Tests.Handlers;

[GenerateClient(RoutePrefix = "prefix")]
public interface IObterProdutoHandler1
{
    public Task<ObterProdutoResponse> GetAsync(ObterProdutoRequest request);
}

[GenerateClient(RoutePrefix = "prefix")]
public interface IObterProdutoHandler2
{
    public Task<ObterProdutoResponse[]> GetAsync(ObterProdutoRequest request);
}

[GenerateClient(RoutePrefix = "prefix")]
public interface IObterProdutoHandler3
{
    public Task<IEnumerable<ObterProdutoResponse>> GetAsync(ObterProdutoRequest request); 
}

[GenerateClient(RoutePrefix = "prefix")]
public interface IObterProdutoHandler4
{
    public Task<ObterProdutoResponse> GetAsync();
}

[GenerateClient(RoutePrefix = "prefix")]
public interface IObterProdutoHandler5
{
    public Task<ObterProdutoResponse[]> GetAsync();
}

[GenerateClient(RoutePrefix = "prefix")]
public interface IObterProdutoHandler6
{
    public Task<IEnumerable<ObterProdutoResponse>> GetAsync();
}


