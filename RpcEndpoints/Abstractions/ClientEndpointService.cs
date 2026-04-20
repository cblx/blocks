using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Json;
using System.Text;

namespace Cblx.Blocks.RpcEndpoints;

internal class ClientEndpointService(HttpClient client, IMemoryCache memoryCache) : IEndpointService
{
    public event EventHandler<RequestSucceededEventArgs>? RequestSucceeded;

    public async Task RequestAsync(ActionEndpoint actionEndpoint)
    {
        var responseMessage = await client.PostAsync(actionEndpoint.Path, null);
        responseMessage.EnsureSuccessStatusCode();
        RequestSucceeded?.Invoke(this, new RequestSucceededEventArgs(actionEndpoint, null, null));
    }
    public async Task RequestAsync<TRequest>(ActionEndpoint<TRequest> actionEndpoint, TRequest request)
    {
        var responseMessage = await client.PostAsJsonAsync(actionEndpoint.Path, request, actionEndpoint.RequestJsonTypeInfo!);
        responseMessage.EnsureSuccessStatusCode();
        RequestSucceeded?.Invoke(this, new RequestSucceededEventArgs(actionEndpoint, request, null));
    }
    public async Task<TResponse> RequestAsync<TResponse>(FuncEndpoint<TResponse> funcEndpoint) => await SendFuncRequestAsync<object, TResponse>(funcEndpoint, null);
    public Task<TResponse> RequestAsync<TRequest, TResponse>(FuncEndpoint<TRequest, TResponse> funcEndpoint, TRequest request) => SendFuncRequestAsync<TRequest, TResponse>(funcEndpoint, request);
    private async Task<TResponse> SendFuncRequestAsync<TRequest, TResponse>(RpcEndpoint<TRequest> endpoint, TRequest? request)
    {
        if (endpoint.Cache != null)
        {
            var cacheKey = endpoint.GetClientCacheKey(request);
            if (memoryCache.TryGetValue(cacheKey, out var cachedResponse))
            {
                return (TResponse)cachedResponse!;
            }
        }
        var responseMessage = endpoint.RequestJsonTypeInfo == null ? await client.PostAsync(endpoint.Path, null)
                                                                   : await client.PostAsJsonAsync(endpoint.Path, request, endpoint.RequestJsonTypeInfo!);
        responseMessage.EnsureSuccessStatusCode();
        var responseTypeInfo = endpoint.ResponseJsonTypeInfo as JsonTypeInfo<TResponse>;
        var response = (await responseMessage.Content.ReadFromJsonAsync(responseTypeInfo!))!;
        if (endpoint.Cache != null)
        {
            memoryCache.Set(endpoint.GetClientCacheKey(request), response, endpoint.Cache.Value);
        }
        RequestSucceeded?.Invoke(this, new RequestSucceededEventArgs(endpoint, request, response));
        return response;
    }

    public async Task MultipartFormDataRequestAsync(ActionEndpoint actionEndpoint, Action<MultipartFormDataContent> configureContent)
    {
        using var content = new MultipartFormDataContent();
        configureContent(content);
        var responseMessage = await client.PostAsync(actionEndpoint.Path, content);
        responseMessage.EnsureSuccessStatusCode();
        RequestSucceeded?.Invoke(this, new RequestSucceededEventArgs(actionEndpoint, null, null));
    }

    public async Task MultipartFormDataRequestAsync<TRequest>(ActionEndpoint<TRequest> actionEndpoint, TRequest request, Action<MultipartFormDataContent> configureContent)
    {
        using var content = new MultipartFormDataContent();
        content.Add(new StringContent(JsonSerializer.Serialize(request, actionEndpoint.RequestJsonTypeInfo!),
                                      Encoding.UTF8,
                                      "application/json"), "json");
        configureContent(content);
        var responseMessage = await client.PostAsync(actionEndpoint.Path, content);
        responseMessage.EnsureSuccessStatusCode();
        RequestSucceeded?.Invoke(this, new RequestSucceededEventArgs(actionEndpoint, request, null));
    }

    public async Task<TResponse> MultipartFormDataRequestAsync<TResponse>(FuncEndpoint<TResponse> funcEndpoint, Action<MultipartFormDataContent> configureContent)
    {
        using var content = new MultipartFormDataContent();
        configureContent(content);
        var responseMessage = await client.PostAsync(funcEndpoint.Path, content);
        responseMessage.EnsureSuccessStatusCode();
        var responseTypeInfo = funcEndpoint.ResponseJsonTypeInfo as JsonTypeInfo<TResponse>;
        var response = (await responseMessage.Content.ReadFromJsonAsync(responseTypeInfo!))!;
        RequestSucceeded?.Invoke(this, new RequestSucceededEventArgs(funcEndpoint, null, response));
        return response;
    }

    public async Task<TResponse> MultipartFormDataRequestAsync<TRequest, TResponse>(FuncEndpoint<TRequest, TResponse> funcEndpoint, TRequest request, Action<MultipartFormDataContent> configureContent)
    {
        using var content = new MultipartFormDataContent();
        content.Add(new StringContent(JsonSerializer.Serialize(request, funcEndpoint.RequestJsonTypeInfo!),
                                      Encoding.UTF8,
                                      "application/json"), "json");
        configureContent(content);
        var responseMessage = await client.PostAsync(funcEndpoint.Path, content);
        responseMessage.EnsureSuccessStatusCode();
        var responseTypeInfo = funcEndpoint.ResponseJsonTypeInfo as JsonTypeInfo<TResponse>;
        var response = (await responseMessage.Content.ReadFromJsonAsync(responseTypeInfo!))!;
        RequestSucceeded?.Invoke(this, new RequestSucceededEventArgs(funcEndpoint, request, response));
        return response;
    }
}