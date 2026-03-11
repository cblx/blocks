namespace Cblx.Blocks.RpcEndpoints.TestsAuxServer.Endpoints.Pets.UpdatePet;

internal class UpdatePetEndpoint() : ActionEndpoint<UpdatePetRequest>(TestAuxSerializerContext.Default.UpdatePetRequest)
{
#if SERVER
    public async Task ExecuteAsync(UpdatePetRequest request)
    {
        await Task.Delay(100);
    }
#endif
}