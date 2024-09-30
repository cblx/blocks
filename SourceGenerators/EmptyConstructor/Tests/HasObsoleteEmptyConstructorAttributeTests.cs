using System.Diagnostics.CodeAnalysis;

namespace Cblx.Blocks.SourceGenerators.EmptyConstructor.Tests;

public class HasObsoleteEmptyConstructorAttributeTests
{
    [Fact]
    public void ClasseAnotadaDeveTerConstrutorVazioComAtributos()
    {
        var ctor = typeof(ClasseComContrutorVazio).GetConstructor(Array.Empty<Type>());
        Assert.NotNull(ctor);
        var customAttributes = ctor!.GetCustomAttributes(false);
        Assert.Contains(customAttributes, attr => attr is ObsoleteAttribute);
        Assert.Contains(customAttributes, attr => attr is ExcludeFromCodeCoverageAttribute);
    }
}
[HasObsoleteEmptyConstructor]
internal partial class ClasseComContrutorVazio { }