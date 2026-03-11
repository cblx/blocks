using System.Text.Json.Serialization.Metadata;

namespace Cblx.Blocks.RpcEndpoints;

internal class EndpointRegistryItem
{
    public required RpcEndpoint Endpoint { get; set; }
    public required JsonTypeInfo? RequestJsonTypeInfo { get; set; }
    public required JsonTypeInfo? ResponseJsonTypeInfo { get; set; }
    public required Delegate Delegate { get; set; }
}
