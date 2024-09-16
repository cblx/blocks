// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

using System.Threading.Tasks;

namespace Cblx.Blocks.Analyzers.Endpoints.Sample;

// If you don't see warnings, build the Analyzers Project.

public class SpaceshipOneEndpoint() : CommandEndpoint()
{
    internal static async Task ExecuteAsync2()
    {
        await Task.CompletedTask;
    }
}

public class SpaceshipOneEndpointWithoutStatic() : CommandEndpoint()
{
    internal async Task ExecuteAsync()
    {
        await Task.CompletedTask;
    }
}

public class SpaceshipOneEndpointWithoutInternal() : CommandEndpoint()
{
    public static async Task ExecuteAsync()
    {
        await Task.CompletedTask;
    }
}

public abstract class EndpointBase(string path)
{
    public string Path => path;
}

public abstract class CommandEndpoint() : EndpointBase("bla");