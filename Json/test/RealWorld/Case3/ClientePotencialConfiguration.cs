namespace Cblx.Blocks.Json.Tests.RealWorld.Case3;

using Cols = TbClientePotencial.Cols;

public class ClientePotencialConfiguration : FlattenJsonConfiguration<ClientePotencial>
{
    public ClientePotencialConfiguration()
    {
        HasJsonPropertyName(c => c.Id, Cols.Id);
    }
}
