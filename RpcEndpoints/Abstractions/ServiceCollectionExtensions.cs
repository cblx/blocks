﻿namespace Cblx.Blocks.RpcEndpoints;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRpcEndpointsClientServices(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddScoped<IEndpointService, ClientEndpointService>();
        return services;
    }
}
