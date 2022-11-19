# blocks
Libs auxiliares e/ou experimentais

## [HandlerBase](https://github.com/cblx/blocks/tree/main/HandlerBase) [![Nuget](https://img.shields.io/nuget/v/Cblx.Blocks.HandlerBase)](https://www.nuget.org/packages/Cblx.Blocks.HandlerBase)

Classe base, Conventions e Source Generators para facilitar a aplicação de um design de código onde temos 1 Controller para cada 1 Handler de Caso de Uso da Aplicação. 

## [SourceGenerators](https://github.com/cblx/blocks/tree/main/SourceGenerators)

### [DependencyInjection](https://github.com/cblx/blocks/tree/main/SourceGenerators/DependencyInjection) [![Nuget](https://img.shields.io/nuget/v/Cblx.Blocks.SourceGenerators.DependencyInjection)](https://www.nuget.org/packages/Cblx.Blocks.SourceGenerators.DependencyInjection)

Gerador de AssemblyMarker por projeto e Gerador de método de extensão para registro de serviços do projeto que estejam anotados com [Scoped], [Transient], [Singleton].

### [EmptyConstructor](https://github.com/cblx/blocks/tree/main/SourceGenerators/EmptyConstructor) [![Nuget](https://img.shields.io/nuget/v/Cblx.Blocks.SourceGenerators.EmptyConstructor)](https://www.nuget.org/packages/Cblx.Blocks.SourceGenerators.EmptyConstructor)

Gerador de Construtores vazios para classes anotadas com os atributos.

## [Testing](https://github.com/cblx/blocks/tree/main/Testing)

Bibliotecas auxiliares para projetos de testes.

### [Xunit](https://github.com/cblx/blocks/tree/main/Testing/Xunit) [![Nuget](https://img.shields.io/nuget/v/Cblx.Blocks.Testing.Xunit)](https://www.nuget.org/packages/Cblx.Blocks.Testing.Xunit)

Traits customizados como [ExternalDependencies] e [LongRunning], para utilizar em filtros de testes.
