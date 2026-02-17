using Cblx.Blocks.RpcEndpoints.TestsAuxServer.Endpoints.Pets.CreatePet;
using Cblx.Blocks.RpcEndpoints.TestsAuxServer.Endpoints.Pets.UpdatePet;
using System.Text.Json.Serialization;

namespace Cblx.Blocks.RpcEndpoints.TestsAuxServer.Endpoints;

[JsonSerializable(typeof(CreatePetRequest))]
[JsonSerializable(typeof(CreatePetResponse))]
[JsonSerializable(typeof(UpdatePetRequest))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
internal partial class TestAuxSerializerContext: JsonSerializerContext;
