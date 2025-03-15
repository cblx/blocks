using Microsoft.Extensions.DependencyInjection;

namespace Cblx.RpcEndpoints;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRpcEndpointsClientServices(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddOutputCache();
        services.AddSingleton<IEndpointService, ServerEndpointService>();
        return services;
    }
}
