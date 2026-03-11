using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text.Json;

namespace Cblx.Blocks.RpcEndpoints;
#pragma warning disable CS0067 // Events are not supported in the server version
/// <summary>
/// Esse serviço é chamado durante server-rendering.
/// Usa reflection, de qualquer forma Blazor Server não tem suporta a AOT => https://learn.microsoft.com/en-us/aspnet/core/fundamentals/native-aot?view=aspnetcore-10.0#aspnet-core-and-native-aot-compatibility
/// </summary>
/// <param name="serviceProvider"></param>
internal class ServerEndpointService(IServiceProvider serviceProvider) : IEndpointService
{
    public event EventHandler<RequestSucceededEventArgs>? RequestSucceeded; // Does not matter in the server version

    private static readonly MethodInfo s_sendRequestVoidAsyncMethodInfo = typeof(ServerEndpointService).GetMethod(nameof(SendRequestVoidAsync), BindingFlags.NonPublic | BindingFlags.Instance)!;
    private static readonly MethodInfo s_sendRequestAsyncMethodInfo = typeof(ServerEndpointService).GetMethod(nameof(SendRequestAsync), BindingFlags.NonPublic | BindingFlags.Instance)!;

    public async Task RequestAsync(ActionEndpoint actionEndpoint) => await SendRequestVoidAsync(actionEndpoint, null);
    public async Task RequestAsync<TRequest>(ActionEndpoint<TRequest> actionEndpoint, TRequest request)
    {
        await ValidateAsync(actionEndpoint.Validator, request);
        await SendRequestVoidAsync(actionEndpoint, request);
    }
    public async Task<TResponse> RequestAsync<TResponse>(FuncEndpoint<TResponse> funcEndpoint) => await SendRequestAsync<object, TResponse>(funcEndpoint, null);
    public async Task<TResponse> RequestAsync<TRequest, TResponse>(FuncEndpoint<TRequest, TResponse> funcEndpoint, TRequest request)
    {
        await ValidateAsync(funcEndpoint.Validator, request);
        return await SendRequestAsync<TRequest, TResponse>(funcEndpoint, request);
    }

    private async Task SendRequestVoidAsync<TRequest>(RpcEndpoint<TRequest> endpoint, TRequest? request)
    {
        var registryItem = EndpointRegistry.Items[endpoint.Path];
        // File as Link no Client. Os Endpoints terão o mesmo Fullname, mas serão tipos diferentes.
        // Fazemos então um "Proxy" da chamada
        if (endpoint.GetType() != registryItem.Endpoint.GetType())
        {
            var serverEndpoint = registryItem.Endpoint;
            object? serverRequest = null;
            if (request != null)
            {
                var json = JsonSerializer.Serialize(request, endpoint.RequestJsonTypeInfo!);
                serverRequest = JsonSerializer.Deserialize(json, registryItem.RequestJsonTypeInfo!);
            }
            var method = s_sendRequestVoidAsyncMethodInfo.MakeGenericMethod(registryItem.RequestJsonTypeInfo?.Type ?? typeof(object));
            var proxyTask = method.Invoke(this, [serverEndpoint, serverRequest]) as Task ?? throw new InvalidOperationException($"The endpoint '{endpoint.GetType().Name}' delegate should return a Task");
            await proxyTask;
            return;
        }
        using var scope = serviceProvider.CreateScope();
        var servicesAndOrRequest = ExtractServicesAndOrRequest(registryItem.Delegate.Method, scope.ServiceProvider, typeof(TRequest), request);
        var task = registryItem.Delegate.DynamicInvoke(servicesAndOrRequest) as Task ?? throw new InvalidOperationException($"The endpoint '{endpoint.GetType().Name}' delegate should return a Task");
        await task;
    }

    private async Task<TResponse> SendRequestAsync<TRequest, TResponse>(RpcEndpoint<TRequest> endpoint, TRequest? request)
    {
        var registryItem = EndpointRegistry.Items[endpoint.Path];
        // File as Link no Client. Os Endpoints terão o mesmo Fullname, mas serão tipos diferentes.
        // Fazemos então um "Proxy" da chamada
        if (endpoint.GetType() != registryItem.Endpoint.GetType())
        {
            var serverEndpoint = registryItem.Endpoint;
            object? serverRequest = null;
            if (request != null)
            {
                var json = JsonSerializer.Serialize(request, endpoint.RequestJsonTypeInfo!);
                serverRequest = JsonSerializer.Deserialize(json, registryItem.RequestJsonTypeInfo!);
            }
            var method = s_sendRequestAsyncMethodInfo.MakeGenericMethod(registryItem.RequestJsonTypeInfo?.Type ?? typeof(object),
                                                                        registryItem.ResponseJsonTypeInfo!.Type);
            var proxyTask = method.Invoke(this, [serverEndpoint, serverRequest]) as Task ?? throw new InvalidOperationException($"The endpoint '{endpoint.GetType().Name}' delegate should return a Task");
            await proxyTask;
            var serverResponse = proxyTask.GetType().GetProperty("Result")!.GetValue(proxyTask);
            return (TResponse)JsonSerializer.Deserialize(JsonSerializer.Serialize(serverResponse, registryItem.ResponseJsonTypeInfo!),
                                               endpoint.ResponseJsonTypeInfo!)!;
        }

        using var scope = serviceProvider.CreateScope();
        var scopedProvider = scope.ServiceProvider;
        IMemoryCache? memoryCache = scopedProvider.GetService<IMemoryCache>();
        var cacheable = endpoint.HasResponse && endpoint.Cache != null;
        var cacheKey = endpoint.GetServerCacheKey(scopedProvider, request);
        if (cacheable)
        {
            if (memoryCache!.TryGetValue(cacheKey, out var cachedResponse))
            {
                return (TResponse)cachedResponse!;
            }
        }
        var servicesAndOrRequest = ExtractServicesAndOrRequest(registryItem.Delegate.Method, scopedProvider, typeof(TRequest), request);
        var task = registryItem.Delegate.DynamicInvoke(servicesAndOrRequest) as Task<TResponse> ?? throw new InvalidOperationException($"The endpoint '{endpoint.GetType().Name}' delegate should return a Task<{typeof(TResponse).Name}>");
        var response = await task;
        if (cacheable)
        {
            memoryCache!.Set(cacheKey, response, endpoint.Cache!.Value);
        }
        return response;
    }

    private static object[] ExtractServicesAndOrRequest(
        MethodBase executeAsyncMethod,
        IServiceProvider serviceProvider,
        Type? requestType,
        object? request)
    {
        return executeAsyncMethod
            .GetParameters()
            .Select(p => GetServiceOrRequest(p, serviceProvider, requestType, request))
            .ToArray();
    }

    private static object GetServiceOrRequest(ParameterInfo parameter, IServiceProvider serviceProvider, Type? requestType, object? request)
    {
        return (parameter.ParameterType == requestType ? request : serviceProvider.GetService(parameter.ParameterType))!;
    }

    private static async Task ValidateAsync<TRequest>(IValidator<TRequest>? validator, TRequest request)
    {
        if (validator is null) return;
        await validator.ValidateAndThrowAsync(request);
    }
}
#pragma warning restore CS0067
