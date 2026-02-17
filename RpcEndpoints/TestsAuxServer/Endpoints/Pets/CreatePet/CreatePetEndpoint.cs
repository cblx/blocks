namespace Cblx.Blocks.RpcEndpoints.TestsAuxServer.Endpoints.Pets.CreatePet;

internal class CreatePetEndpoint() : FuncEndpoint<CreatePetRequest, CreatePetResponse>(TestAuxSerializerContext.Default.CreatePetRequest,
                                                                                       TestAuxSerializerContext.Default.CreatePetResponse)
{
#if SERVER
    public async Task<CreatePetResponse> ExecuteAsync(CreatePetRequest request)
    {
        await Task.Delay(100);
        return new CreatePetResponse { Id = Guid.NewGuid() };
    }
#endif
}