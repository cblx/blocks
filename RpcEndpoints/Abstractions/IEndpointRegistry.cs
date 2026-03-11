namespace Cblx.Blocks.RpcEndpoints;

public interface IEndpointRegistry
{
    IEndpointRegistry Register<TRequest>(RpcEndpoint<TRequest> rpcEndpoint, Delegate executor);
    IEndpointRegistry Register<TEndpoint>(TEndpoint endpoint, Func<TEndpoint, Delegate> executorAccessor) where TEndpoint : RpcEndpoint;
    IEndpointRegistry Register<TEndpoint>(Func<TEndpoint, Delegate> executorAccessor) where TEndpoint : RpcEndpoint, new();
    IEndpointRegistry Register(Delegate executor);
}
