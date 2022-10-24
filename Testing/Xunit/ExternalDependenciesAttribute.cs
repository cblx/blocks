using Xunit.Sdk;

namespace Cblx.Blocks.Testing.Xunit;

/// <summary>
/// The test connects to some external dependency
/// </summary>
[TraitDiscoverer("Cblx.Blocks.Testing.Xunit.ExternalDependenciesDiscoverer", "Cblx.Blocks.Testing.Xunit")]
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class ExternalDependenciesAttribute : Attribute, ITraitAttribute
{
}
