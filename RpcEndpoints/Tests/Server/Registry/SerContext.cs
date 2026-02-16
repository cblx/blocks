using System.Text.Json.Serialization;

namespace Cblx.Blocks.RpcEndpoints.Tests.Server.Registry;

[JsonSerializable(typeof(Request))]
public partial class SerContext : JsonSerializerContext;