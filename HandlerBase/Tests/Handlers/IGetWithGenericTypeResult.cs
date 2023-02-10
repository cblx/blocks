

namespace Cblx.Blocks.HandlerBase.Tests.Handlers;


[GenerateClient]
public interface IGetWithGenericTypeResult
{
    Task<WithGeneric<GeneretorEnumType>[]> GetAsync();
}

[GenerateClient]
public interface IGetWithGenericTypeIntResult
{
    ValueTask<WithGeneric<int>[]> GetAsync();
}


[GenerateClient]
public interface IGetWithGenericTypeObjectResult
{
    Task<WithGeneric<int>> GetAsync();
}

[GenerateClient]
public interface IGetWithGenericTypeProductResult
{
    Task<WithGeneric<ObterProdutoResponse>> GetAsync();
}