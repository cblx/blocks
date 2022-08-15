namespace Cblx.Blocks.HandlerBase.Tests.Handlers;

//[GenerateClient(RoutePrefix = "prefix")]
//public interface IObterProdutoHandler1
//{
//    public Task<ObterProdutoResponse> GetAsync(ObterProdutoRequest request);
//}

//[GenerateClient(RoutePrefix = "prefix")] 
//public interface IObterProdutoHandler2
//{
//    public Task<ObterProdutoResponse[]> GetAsync(ObterProdutoRequest request);
//}

//[GenerateClient(RoutePrefix = "prefix")]
//public interface IObterProdutoHandler3
//{
//    public Task<IEnumerable<ObterProdutoResponse>> GetAsync(ObterProdutoRequest request); 
//}

//[GenerateClient(RoutePrefix = "prefix")]
//public interface IObterProdutoHandler4
//{
//    public Task<ObterProdutoResponse> GetAsync();
//}

//[GenerateClient(RoutePrefix = "prefix")]
//public interface IObterProdutoHandler5
//{
//    public Task<ObterProdutoResponse[]> GetAsync();
//}

//[GenerateClient(RoutePrefix = "prefix")]
//public interface IObterProdutoHandler6
//{
//    public Task<IEnumerable<ObterProdutoResponse>> GetAsync();
//}


//[GenerateClient()]
//public interface IObterProdutoHandler7
//{
//    public Task<ObterProdutoResponse> GetAsync(ObterProdutoRequest request);
//}

//[GenerateClient()]
//public interface IObterProdutoHandler8
//{
//    public Task<ObterProdutoResponse[]> GetAsync(ObterProdutoRequest request);
//}

//[GenerateClient()]
//public interface IObterProdutoHandler9
//{
//    public Task<IEnumerable<ObterProdutoResponse>> GetAsync(ObterProdutoRequest request);
//}

[GenerateClient()]
public interface ICriarProdutoHandle1
{
    public Task PostAsync(ObterProdutoRequest request);
}

[GenerateClient()]
public interface ICriarProdutoHandler2
{
    public Task<ObterProdutoResponse> PostAsync(ObterProdutoRequest request);
}

[GenerateClient()]
public interface IDeleteProdutoHandle1
{
    public Task DeleteAsync(ObterProdutoRequest request);
}

[GenerateClient()]
public interface IDeleteProdutoHandler2
{
    public Task<ObterProdutoResponse> DeleteAsync(ObterProdutoRequest request);
}


