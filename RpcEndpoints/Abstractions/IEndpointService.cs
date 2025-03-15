namespace Cblx.RpcEndpoints;

public interface IEndpointService
{
    Task RequestAsync(ActionEndpoint actionEndpoint);
    Task RequestAsync<TRequest>(ActionEndpoint<TRequest> actionEndpoint, TRequest request);
    Task<TResponse> RequestAsync<TResponse>(FuncEndpoint<TResponse> funcEndpoint);
    Task<TResponse> RequestAsync<TRequest, TResponse>(FuncEndpoint<TRequest, TResponse> funcEndpoint, TRequest request);
    event EventHandler<RequestSucceededEventArgs>? RequestSucceeded;
}
