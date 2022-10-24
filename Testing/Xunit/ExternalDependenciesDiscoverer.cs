using Xunit.Abstractions;
using Xunit.Sdk;

namespace Cblx.Blocks.Testing.Xunit;

public class ExternalDependenciesDiscoverer : ITraitDiscoverer
{
    public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
    {
        yield return new KeyValuePair<string, string>("Category", "ExternalDependencies");
    }
}
