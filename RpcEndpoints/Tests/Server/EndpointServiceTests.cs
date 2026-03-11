extern alias Client;
extern alias Server;

using ClientPets = Client.Cblx.Blocks.RpcEndpoints.TestsAuxServer.Endpoints.Pets;
using ServerPets = Server.Cblx.Blocks.RpcEndpoints.TestsAuxServer.Endpoints.Pets;

using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;
namespace Cblx.Blocks.RpcEndpoints.Tests.Server;

public class EndpointServiceTests
{
    [Fact]
    public async Task RequestTest()
    {
        ICollection<EndpointDataSource> dataSources = [];
        var registry = new EndpointRegistry(Mock.Of<IEndpointRouteBuilder>(m => m.DataSources == dataSources));
        var endpointService = new ServerEndpointService(new ServiceCollection().BuildServiceProvider());
        registry.Register<ServerPets.CreatePet.CreatePetEndpoint>(e => e.ExecuteAsync);
        var response = await endpointService.RequestAsync(new ClientPets.CreatePet.CreatePetEndpoint(),
                                                          new ClientPets.CreatePet.CreatePetRequest { Name = "Bobby" });
    }

    [Fact]
    public async Task EmptyRequestTest()
    {
        ICollection<EndpointDataSource> dataSources = [];
        var registry = new EndpointRegistry(Mock.Of<IEndpointRouteBuilder>(m => m.DataSources == dataSources));
        var endpointService = new ServerEndpointService(new ServiceCollection().BuildServiceProvider());
        registry.Register<ServerPets.ListPets.ListPetsEndpoint>(e => e.ExecuteAsync);
        var response = await endpointService.RequestAsync(new ClientPets.ListPets.ListPetsEndpoint());
    }

    [Fact]
    public async Task EmptyRequestVoidTest()
    {
        ICollection<EndpointDataSource> dataSources = [];
        var registry = new EndpointRegistry(Mock.Of<IEndpointRouteBuilder>(m => m.DataSources == dataSources));
        var endpointService = new ServerEndpointService(new ServiceCollection().BuildServiceProvider());
        registry.Register<ServerPets.PingPets.PingPetsEndpoint>(e => e.ExecuteAsync);
        await endpointService.RequestAsync(new ClientPets.PingPets.PingPetsEndpoint());
    }

    [Fact]
    public async Task RequestVoidTest()
    {
        ICollection<EndpointDataSource> dataSources = [];
        var registry = new EndpointRegistry(Mock.Of<IEndpointRouteBuilder>(m => m.DataSources == dataSources));
        var endpointService = new ServerEndpointService(new ServiceCollection().BuildServiceProvider());
        registry.Register<ServerPets.UpdatePet.UpdatePetEndpoint>(e => e.ExecuteAsync);
        await endpointService.RequestAsync(new ClientPets.UpdatePet.UpdatePetEndpoint(),
                                          new ClientPets.UpdatePet.UpdatePetRequest { NewName = "Bobby" });
    }
}
