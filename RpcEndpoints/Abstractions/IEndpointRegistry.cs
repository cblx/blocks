
using System.Linq.Expressions;

namespace Cblx.Blocks.RpcEndpoints;

public interface IEndpointRegistry
{
    [Obsolete("Removero override do 'Delegate' do Endpoint, e explicitar o método de execução no Register.")]
    IEndpointRegistry Register<TRequest>(RpcEndpoint<TRequest> rpcEndpoint);
    IEndpointRegistry Register<TRequest>(RpcEndpoint<TRequest> rpcEndpoint, Delegate executor);
    IEndpointRegistry Register<TEndpoint>(TEndpoint endpoint, Func<TEndpoint, Delegate> executorAccessor) where TEndpoint : RpcEndpoint;
    IEndpointRegistry Register<TEndpoint>(Func<TEndpoint, Delegate> executorAccessor) where TEndpoint : RpcEndpoint, new();
    IEndpointRegistry Register(Delegate executor);
}
