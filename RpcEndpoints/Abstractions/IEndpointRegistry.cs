
using System.Linq.Expressions;

namespace Cblx.Blocks.RpcEndpoints;

public interface IEndpointRegistry
{
    [Obsolete]
    IEndpointRegistry Register<TRequest>(RpcEndpoint<TRequest> rpcEndpoint);
    IEndpointRegistry Register<TRequest>(RpcEndpoint<TRequest> rpcEndpoint, Delegate executor);
    IEndpointRegistry Register<TEndpoint>(TEndpoint endpoint, Func<TEndpoint, Delegate> executorAccessor) where TEndpoint : RpcEndpoint;
    IEndpointRegistry Register<TEndpoint>(Func<TEndpoint, Delegate> executorAccessor) where TEndpoint : RpcEndpoint, new();
}
