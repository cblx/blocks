using Microsoft.AspNetCore.Routing;
using Moq;
using Xunit;

namespace Cblx.Blocks.RpcEndpoints.Tests.Server.Registry;


public class EndpointRegistryTests
{
    

    [Fact]
    public void RegisterTest()
    {
        ICollection<EndpointDataSource> dataSources =[];
        var registry = new EndpointRegistry(Mock.Of<IEndpointRouteBuilder>(m => m.DataSources == dataSources));
        registry.Register(new ActNoParamEndpoint(), ActNoParamEndpoint.ExecuteAsync)
                //.Register(new ActWithRequestParamEndpoint(), ActWithRequestParamEndpoint.ExecuteAsync)
                .Register(new ActWithServiceEndpoint(), ActWithServiceEndpoint.ExecuteAsync)

                // Estático. O ExecuteAsync poderia estar em qualquer outro lugar.
                .Register(new ActWithServiceAndRequestEndpoint(), ActWithServiceAndRequestEndpoint.ExecuteAsync)
                // Instância. O ExecuteAsync tem de estar na própria classe.
                .Register(new ActWithRequestParamEndpoint(), e => e.ExecuteAsync)
                // Instância, com new(). O ExecuteAsync tem de estar na própria classe.
                .Register<ActWithRequestParamEndpoint>(e => e.ExecuteAsync);


    }


    class ActNoParamEndpoint() : ActionEndpoint()
    {
        internal static Task ExecuteAsync() => Task.CompletedTask;
    }

    class ActWithRequestParamEndpoint() : ActionEndpoint<Request>(SerContext.Default.Request)
    {
        internal Task ExecuteAsync(Request request) => Task.CompletedTask;
    }

    class ActWithServiceEndpoint() : ActionEndpoint()
    {
        internal static Task ExecuteAsync(IServiceProvider serviceProvider) => Task.CompletedTask;
    }

    class ActWithServiceAndRequestEndpoint() : ActionEndpoint<Request>(SerContext.Default.Request)
    {
        internal static Task ExecuteAsync(Request request, IServiceProvider serviceProvider) => Task.CompletedTask;
    }
}
