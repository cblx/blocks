namespace Cblx.Blocks.RpcEndpoints;

public abstract class FuncAsyncEnumerableEndpoint<TResponseItem>(
    JsonTypeInfo<TResponseItem> responseItemTypeInfo,
    bool allowAnonymous = false//,
                               // Cache precisaria ter yield também ao retornar. Pensar melhor
    ) : RpcEndpoint<object>(null, responseItemTypeInfo, null, allowAnonymous), IFuncAsyncEnumerable;



public abstract class FuncAsyncEnumerableEndpoint<TRequest, TResponseItem>(
    JsonTypeInfo<TRequest> requestTypeInfo,
    JsonTypeInfo<TResponseItem> responseItemTypeInfo,
    IValidator<TRequest>? validator = null,
    bool allowAnonymous = false//,
                               // Cache precisaria ter yield também ao retornar. Pensar melhor
    ) : RpcEndpoint<TRequest>(requestTypeInfo, responseItemTypeInfo, validator, allowAnonymous), IFuncAsyncEnumerable;


public interface IFuncAsyncEnumerable;