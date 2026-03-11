namespace Cblx.Blocks.RpcEndpoints.TestsAuxServer.Endpoints.Pets.PingPets;

internal class PingPetsEndpoint() : ActionEndpoint()
{
#if SERVER
    public async Task ExecuteAsync()
    {
        await Task.Delay(100);
    }
#endif
}
