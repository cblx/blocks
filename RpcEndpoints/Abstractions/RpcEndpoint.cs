namespace Cblx.Blocks.RpcEndpoints;

public abstract class RpcEndpoint { }

// Caso precisemos suportar algo de upload, acho que vale olhar com cuidado isso:
// https://learn.microsoft.com/en-us/aspnet/core/release-notes/aspnetcore-7.0?source=recommendations&view=aspnetcore-7.0#file-uploads-using-iformfile-and-iformfilecollection
public abstract class RpcEndpoint<TRequest>(
    JsonTypeInfo<TRequest>? requestJsonTypeInfo,
    JsonTypeInfo? responseJsonTypeInfo,
    IValidator<TRequest>? validator = null,
    bool allowAnonymous = false,
    TimeSpan? cache = null): RpcEndpoint
{
    public bool AllowAnonymous { get; } = allowAnonymous;
    public TimeSpan? Cache { get; } = cache;
    public JsonTypeInfo? ResponseJsonTypeInfo { get; } = responseJsonTypeInfo;
    public bool HasResponse => ResponseJsonTypeInfo is not null;
    public JsonTypeInfo<TRequest>? RequestJsonTypeInfo { get; } = requestJsonTypeInfo;
    public bool HasRequest => RequestJsonTypeInfo is not null;
    public IValidator<TRequest>? Validator { get; } = validator;
    public string Path => $"rpc/{GetType().FullName!.Replace(".", "/")}";

    [Obsolete("Referenciar o método de execução diretamento no registry.Register(...)")]
    protected virtual Delegate Delegate { get; } = null!;

    protected virtual Func<IServiceProvider, string>? VaryServerCacheBy { get; }

    [Obsolete("Será removido.")]
    internal Delegate GetDelegate() => Delegate;
    internal bool VariesServerCache => VaryServerCacheBy is not null;
    internal string GetServerCacheKey(IServiceProvider serviceProvider, TRequest? request)
    {
        var requestJson = request == null ? "" : JsonSerializer.Serialize(request, RequestJsonTypeInfo!);
        return $"{Path}|r={requestJson}|v={VaryServerCacheBy?.Invoke(serviceProvider)}";
    }

    internal string GetClientCacheKey(TRequest? request)
    {
        var requestJson = request == null ? "" : JsonSerializer.Serialize(request, RequestJsonTypeInfo!);
        return $"{Path}|r={requestJson}";
    }
}
