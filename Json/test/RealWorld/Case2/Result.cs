using System.Text.Json.Serialization;

namespace Cblx.Blocks.Json.Tests.RealWorld.Case2;

public class Result
{
    [JsonPropertyName("value")]
    public IEnumerable<ClientePotencial> Value { get; set; } = Array.Empty<ClientePotencial>();
}