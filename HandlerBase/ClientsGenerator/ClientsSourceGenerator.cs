using Cblx.Blocks.Configuration;
using Cblx.Blocks.Factories;
using Cblx.Blocks.Finders;
using Cblx.Blocks.Templates;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
//using System.Diagnostics;
using System.Linq;

namespace Cblx.Blocks;

[Generator]
public class ClientsSourceGenerator : ISourceGenerator
{
  
    public void Execute(GeneratorExecutionContext context)
    {
        //Register here - aditional source code suporte
        context.AddSource("QueryStringHelper.g.cs", QueryStringHelperTemplate.Source);

//#if DEBUG
//        if (!Debugger.IsAttached)
//        {
//            Debugger.Launch();
//        }
//#endif

        if (context.SyntaxReceiver is not HandlerFinder handlerFinder) return;

        CreateAndRegisterClientsInContext(handlerFinder.Handlers.ToArray(), context);
    }    

    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new HandlerFinder());
    }

    private static void CreateAndRegisterClientsInContext(InterfaceDeclarationSyntax[] interfaceDeclarations, GeneratorExecutionContext context)
    {
        var clientGeneratorSettingBuilder = new ClientGeneratorSettingsBuilder(context.Compilation.Assembly);
        var handlerFactory = new HandlerDeclarationFactory(clientGeneratorSettingBuilder, context);

        foreach (InterfaceDeclarationSyntax interfaceDeclaration in interfaceDeclarations)
        {
            var handler = handlerFactory.CreateOrDefault(interfaceDeclaration);
            if (handler is null) continue;

            var code = HandlerClientTamplate.Create(handler);
            context.AddSource($"{handler.ImplementationName}Client.g.cs", code);
        }
    }
}
