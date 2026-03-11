namespace Cblx.Blocks.RpcEndpoints.TestsAuxServer.Endpoints.Pets.ListPets;

internal class ListPetsEndpoint() : FuncEndpoint<PetDto[]>(TestAuxSerializerContext.Default.PetDtoArray)
{
#if SERVER
    public async Task<PetDto[]> ExecuteAsync()
    {
        await Task.Delay(100);
        return [];
    }
#endif

}
