namespace Cblx.Blocks.Configuration;

internal sealed class ClientGeneratorSettings
{
    public ClientGeneratorSettings(string? routePrefix)
    {
        RoutePrefix = routePrefix;
    }

    public string? RoutePrefix { get; private set; }
}
