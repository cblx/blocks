namespace Cblx.Blocks.HandlerBase.Tests.Handlers;


[GenerateClient]
public interface IGetWithGenericTypeResult
{
    Task<WithGeneric<GeneretorEnumType>[]> GetAsync();
}

[GenerateClient]
public interface IGetWithGenericTypeIntResult
{
    Task<WithGeneric<int>[]> GetAsync();
}
