using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace Cblx.Blocks.RpcEndpoints;

public class RpcCacheFilter<TRequest>(RpcEndpoint<TRequest> rpcEndpoint) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if (rpcEndpoint.HasResponse && rpcEndpoint.Cache != null)
        {
            var serviceProvider = context.HttpContext.RequestServices;
            var memoryCache = serviceProvider.GetRequiredService<IMemoryCache>();
            string cacheKey;
            if (rpcEndpoint.HasRequest)
            {
                cacheKey = rpcEndpoint.GetServerCacheKey(serviceProvider, context.GetArgument<TRequest>(0));
            }
            else
            {
                cacheKey = rpcEndpoint.GetServerCacheKey(serviceProvider, default);
            }
            if (memoryCache.TryGetValue(cacheKey, out var cachedResponse))
            {
                return cachedResponse;
            }
            var result = await next.Invoke(context);
            memoryCache.Set(cacheKey, result, rpcEndpoint.Cache.Value);
            return result;
        }
        return await next.Invoke(context);
    }
}
