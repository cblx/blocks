namespace Cblx.RpcEndpoints;

public interface IEndpointRegistry
{
    IEndpointRegistry Register<TRequest>(RpcEndpoint<TRequest> rpcEndpoint);
}
