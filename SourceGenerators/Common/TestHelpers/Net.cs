using Microsoft.CodeAnalysis.Testing;
using System.Collections.Immutable;

namespace Cblx.Blocks.SourceGenerators;
// Currently, .NET7 is missing from ReferenceAssemblies.Net.Net70.
// Remove this class when it's officaly available
public static class Net
{
    private static readonly Lazy<ReferenceAssemblies> _lazyNet70 = new(() =>
        new ReferenceAssemblies(
            "net7.0",
            new PackageIdentity(
                "Microsoft.NETCore.App.Ref",
                "7.0.0"),
            Path.Combine("ref", "net7.0")));
    public static ReferenceAssemblies Net70 => _lazyNet70.Value;

    private static readonly Lazy<ReferenceAssemblies> _lazyNet70Windows = new(() =>
        Net70.AddPackages(
            ImmutableArray.Create(
                new PackageIdentity("Microsoft.WindowsDesktop.App.Ref", "7.0.0"))));
    public static ReferenceAssemblies Net70Windows => _lazyNet70Windows.Value;
}