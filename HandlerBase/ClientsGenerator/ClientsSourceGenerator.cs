using Cblx.Blocks.Configuration;
using Cblx.Blocks.Factories;
using Cblx.Blocks.Finders;
using Cblx.Blocks.Helpers;
using Cblx.Blocks.Templates;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Cblx.Blocks;

[Generator]
public class ClientsSourceGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {

#if DEBUG
        if (!Debugger.IsAttached)
        {
            Debugger.Launch();
        }
#endif

        if (context.SyntaxReceiver is not HandlerFinder handlerFinder)
        {
            return;
        }

        CreateAndRegisterClientsInContext(handlerFinder.Handlers.ToArray(), context);
    }

    public void Initialize(GeneratorInitializationContext context)
        => context.RegisterForSyntaxNotifications(() => new HandlerFinder());


    private static void CreateAndRegisterClientsInContext(
        IEnumerable<InterfaceDeclarationSyntax> interfaceDeclarations,
        GeneratorExecutionContext context)
    {
        var clientGeneratorSettingBuilder = new ClientGeneratorSettingsBuilder(context.Compilation.Assembly);
        var handlerFactory = new HandlerDeclarationFactory(context, clientGeneratorSettingBuilder);

        ServiceCollectionTemplate.Clean();

        foreach (var interfaceDeclaration in interfaceDeclarations)
        {
            var handler = handlerFactory.CreateOrDefault(interfaceDeclaration);
            if (handler is null)
            {
                continue;
            }

            var code = HandlerClientTemplate.Create(handler);

            ServiceCollectionTemplate.AddScoped(handler);
            context.AddSource($"{handler.ImplementationName}Client.g.cs", code);
        }

        var assemblyName = context.Compilation.AssemblyName!;
        var addServicesName = $"Add{assemblyName.Replace(".", "")}ClientHandlers";
        var codeForService = ServiceCollectionTemplate.CreateOrDefault(assemblyName, addServicesName);

        if (string.IsNullOrEmpty(codeForService))
        {
            return;
        }

        context.AddSource("ServiceCollectionExtensionsForClientHandlers.g.cs", codeForService!);
    }
}
