# Roslyn Analyzers Endpoints

This project contains a sample analyzer and code fix provider for the `Endpoint.ExecuteAsync` method. The analyzer reports the usage of `Endpoint.ExecuteAsync` method, and the code fix providers replace it with `Endpoint.InternalExecuteAsync` or `Endpoint.StaticExecuteAsync`.

## Content
### Cblx.Blocks.Analyzers.Endpoints
A .NET Standard project with implementations of sample analyzers and code fix providers.
**You must build this project to see the results (warnings) in the IDE.**

- [EndpointExecuteAsyncAnalyzer.cs](EndpointExecuteAsyncAnalyzer.cs): An analyzer that reports the usage of `Endpoint.ExecuteAsync` method.
- [EndpointInternalExecuteAsyncProvider.cs](EndpointInternalExecuteAsyncProvider.cs): A code fix that replaces `Endpoint.ExecuteAsync` with `Endpoint.InternalExecuteAsync`.
- [EndpointStaticExecuteAsyncProvider.cs](EndpointStaticExecuteAsyncProvider.cs): A code fix that replaces `Endpoint.ExecuteAsync` with `Endpoint.StaticExecuteAsync`.

### Cblx.Blocks.Analyzers.Endpoints.Sample
A project that references the sample analyzers. Note the parameters of `ProjectReference` in [Cblx.Blocks.Analyzers.Endpoints.Sample.csproj](../Sample/Cblx.Blocks.Analyzers.Endpoints.Sample.csproj), they make sure that the project is referenced as a set of analyzers. 

### Cblx.Blocks.Analyzers.Endpoints.Tests
Unit tests for the sample analyzers and code fix provider. The easiest way to develop language-related features is to start with unit tests.

## How To?
### How to debug?
- Use the [launchSettings.json](Properties/launchSettings.json) profile.
- Debug tests.

### How can I determine which syntax nodes I should expect?
Consider installing the Roslyn syntax tree viewer plugin [Rossynt](https://plugins.jetbrains.com/plugin/16902-rossynt/).

### Learn more about wiring analyzers
The complete set of information is available at [roslyn github repo wiki](https://github.com/dotnet/roslyn/blob/main/docs/wiki/README.md).