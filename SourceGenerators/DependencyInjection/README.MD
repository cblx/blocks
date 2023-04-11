[![Nuget](https://img.shields.io/nuget/v/Cblx.Blocks.SourceGenerators.DependencyInjection)](https://www.nuget.org/packages/Cblx.Blocks.SourceGenerators.DependencyInjection)

SourceGenerator para criar métodos de extensão no `IServiceCollection` para registro de serviços anotados via atributos.

Ex, para registrar um serviço:

```csharp
namespace MeuAssembly;
[Scoped<IMyService>]
public class MyService : IMyService {}
```

Os métodos de extensão serão criados utilizando o nome do Assembly:

```csharp
services
	.AddMeuAssemblyServices()
	.AddMeuOutroAssemblyServices();
```

Também são criados AssemblyMarkers para os projetos:

`MeuAssembly.g.cs`
```csharp
public static class MeuAssemblyMarker{
	public static Assembly Assembly = typeof(MeuAssemblyMarker).Assembly;
}
```

Ex de uso:

```
// Como exemplo, para registro de validators do FluentValidation
services.AddValildatorsFromAssembly(MeuAssemblyMarker.Assembly);
```
