using Xunit.Sdk;

namespace Cblx.Blocks.Testing.Xunit;

/// <summary>
/// The test runs for a long time
/// </summary>
[TraitDiscoverer("Cblx.Blocks.Testing.Xunit.LongRunningDiscoverer", "Cblx.Blocks.Testing.Xunit")]
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class LongRunningAttribute : Attribute, ITraitAttribute
{
}
