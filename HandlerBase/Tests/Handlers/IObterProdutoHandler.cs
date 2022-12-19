namespace Cblx.Blocks.HandlerBase.Tests.Handlers;

[GenerateClient(RoutePrefix = "prefix")]
public interface IObterProdutoHandler1
{
    Task<ObterProdutoResponse> GetAsync(ObterProdutoRequest request);
}

[GenerateClient(RoutePrefix = "prefix")] 
public interface IObterProdutoHandler2
{
    Task<ObterProdutoResponse[]> GetAsync(ObterProdutoRequest request);
}

[GenerateClient(RoutePrefix = "prefix")]
public interface IObterProdutoHandler3
{
    Task<IEnumerable<ObterProdutoResponse>> GetAsync(ObterProdutoRequest request); 
}

[GenerateClient(RoutePrefix = "prefix")]
public interface IObterProdutoHandler4
{
    Task<ObterProdutoResponse> GetAsync();
}

[GenerateClient(RoutePrefix = "prefix")]
public interface IObterProdutoHandler5
{
    Task<ObterProdutoResponse[]> GetAsync();
}

[GenerateClient(RoutePrefix = "prefix")]
public interface IObterProdutoHandler6
{
    Task<IEnumerable<ObterProdutoResponse>> GetAsync();
}

[GenerateClient()]
public interface IObterProdutoHandler7
{
    Task<ObterProdutoResponse> GetAsync(ObterProdutoRequest request);
}

[GenerateClient()]
public interface IObterProdutoHandler8
{
    Task<ObterProdutoResponse[]> GetAsync(ObterProdutoRequest request);
}

[GenerateClient()]
public interface IObterProdutoHandler9
{
    Task<IEnumerable<ObterProdutoResponse>> GetAsync(ObterProdutoRequest request);
}

[GenerateClient()]
public interface ICriarProdutoHandle1
{
    Task PostAsync(ObterProdutoRequest request);
}

[GenerateClient()]
public interface ICriarProdutoHandler2
{
    Task<ObterProdutoResponse> PostAsync(ObterProdutoRequest request);
}

[GenerateClient()]
public interface IDeleteProdutoHandle1
{
    Task DeleteAsync(ObterProdutoRequest request);
}

[GenerateClient()]
public interface IDeleteProdutoHandler2
{
    Task<ObterProdutoResponse> DeleteAsync(ObterProdutoRequest request);
}


