using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Cblx.Blocks.RpcEndpoints;

public static class MapEndpointExtensions
{
    /// <summary>
    /// RECOMMENDED ACTION: Add the .UseOutputCache() middleware to the pipeline, somewhere you like before this method.
    /// </summary>
    /// <param name="endpoints"></param>
    /// <param name="registrators"></param>
    /// <returns></returns>
    public static IEndpointRouteBuilder MapEndpoints(
        this IEndpointRouteBuilder endpoints,
        params Action<IEndpointRegistry>[] registrators)
    {
        var registry = new EndpointRegistry(endpoints);
        registrators.ToList().ForEach(r => r(registry));
        return endpoints;
    }

    internal static IEndpointRouteBuilder MapRpcEndpoint<TRequest>(this IEndpointRouteBuilder endpoints, RpcEndpoint<TRequest> endpoint)
    {
        var routeBuilder = endpoints.MapPost(endpoint.Path, endpoint.GetDelegate());
        if (endpoint.Cache != null)
        {
            // If response does not vary by custom logic, then we can use output caching
            if (!endpoint.VariesServerCache)
            {
                routeBuilder.CacheOutput(pBuilder => pBuilder.AddPolicy<RpcCachePolicy>().Expire(endpoint.Cache.Value), excludeDefaultPolicy: true);
            }
            routeBuilder.AddEndpointFilter(new RpcCacheFilter<TRequest>(endpoint));
        }
        if (endpoint.AllowAnonymous)
        {
            routeBuilder.AllowAnonymous();
        }
        else
        {
            routeBuilder.RequireAuthorization();
        }
        if (endpoint.Validator is not null)
        {
            routeBuilder.AddValidator(endpoint.Validator);
        }
        return endpoints;
    }
}