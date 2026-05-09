using Microsoft.AspNetCore.Routing;

namespace Cblx.Blocks.RpcEndpoints;

public static class MapEndpointExtensions
{
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
        var routeBuilder = endpoints.MapPost(endpoint.Path, EndpointRegistry.Items[endpoint.Path].Delegate);
        if (endpoint.Cache != null)
        {
            // If response does not vary by custom logic, then we can use output caching
            // Isso parece ter dado problema no dia 19/01/2026. (não é certeza, mas é suspeito)
            // Basicamente passou a ocorrer erro 400 nos endpoints com cache habilitado, após uma publicação, e retornando corpo vazio.
            // Acredito que foi feito esse outputcache pois em tese ele seria bem mais rápido de entregar
            // do que usar o memorycache.
            // Acabou ficando duas camadas de cache, "outputcache" e "filter+memorycache".
            // Pensando melhor, o ganho maior do cache não é ter as coisas de forma bruta
            // mas sim evitar acessar o banco de dados ou fazer cálculos pesados.
            // Então, para simplificar a arquitetura, vou remover o outputcache.
            //if (!endpoint.VariesServerCache)
            //{
            //    routeBuilder.CacheOutput(pBuilder => pBuilder.AddPolicy<RpcCachePolicy>().Expire(endpoint.Cache.Value), excludeDefaultPolicy: true);
            //}
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