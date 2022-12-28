using System.Collections.Generic;
using Cblx.Blocks.Configuration;
using Cblx.Blocks.Factories;
using Cblx.Blocks.Finders;
using Cblx.Blocks.Templates;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics;
using System.Linq;
using Cblx.Blocks.Helpers;

namespace Cblx.Blocks;

[Generator]
public class ClientsSourceGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {

#if DEBUG
        if (!Debugger.IsAttached)
        {
            //Debugger.Launch();
        }
#endif

        if (context.SyntaxReceiver is not HandlerFinder handlerFinder) return;

        CreateAndRegisterClientsInContext(handlerFinder.Handlers.ToArray(), context);
    }    

    public void Initialize(GeneratorInitializationContext context) 
        => context.RegisterForSyntaxNotifications(() => new HandlerFinder());


    private static void CreateAndRegisterClientsInContext(
        IEnumerable<InterfaceDeclarationSyntax> interfaceDeclarations, 
        GeneratorExecutionContext context)
    {
        CodeHelpers.ChangeGeneratorExecutionContext(context);

        var clientGeneratorSettingBuilder = new ClientGeneratorSettingsBuilder(CodeHelpers.GetAssemblySymbol());
        var handlerFactory = new HandlerDeclarationFactory(clientGeneratorSettingBuilder);

        ServiceCollectionTemplate.Clean();

        foreach (var interfaceDeclaration in interfaceDeclarations)
        {
            var handler = handlerFactory.CreateOrDefault(interfaceDeclaration);
            if (handler is null) continue;

            var code = HandlerClientTemplate.Create(handler);
            
            ServiceCollectionTemplate.AddScoped(handler);
            CodeHelpers.AddSource($"{handler.ImplementationName}Client.g.cs", code);
        }
        
        var assemblyName = CodeHelpers.GetAssemblyName()!;
        var addServicesName = $"Add{assemblyName.Replace(".", "")}ClientHandlers";
        var codeForService = ServiceCollectionTemplate.CreateOrDefault(assemblyName, addServicesName);

        if (string.IsNullOrEmpty(codeForService))
        {
            CodeHelpers.RemoveGeneratorExecutionContext();
            return;
        }

        CodeHelpers.AddSource("ServiceCollectionExtensionsForClientHandlers.g.cs", codeForService!);
        CodeHelpers.RemoveGeneratorExecutionContext();

    }
}
