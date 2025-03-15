namespace Cblx.RpcEndpoints;

public class RequestSucceededEventArgs(object endpoint, object? request, object? response) : EventArgs
{
    public object Endpoint { get; } = endpoint;
    public object? Request { get; } = request;
    public object? Response { get; } = response;
}