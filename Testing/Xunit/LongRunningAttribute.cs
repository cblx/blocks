using Xunit.Sdk;

namespace Cblx.Blocks.Testing.Xunit;

/// <summary>
/// Teste conectado a um Dynamics
/// </summary>
[TraitDiscoverer("Cblx.Blocks.Testing.Xunit.LongRunningDiscoverer", "Cblx.Blocks.Testing.Xunit")]
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class LongRunningAttribute : Attribute, ITraitAttribute
{
}
