using Microsoft.Extensions.DependencyInjection;

namespace Cblx.Blocks.RpcEndpoints;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRpcEndpointsServerServices(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddOutputCache();
        services.AddScoped<IEndpointService, ServerEndpointService>();
        return services;
    }
}
