using Cblx.Blocks.Configuration;
using Cblx.Blocks.Extensions;
using Cblx.Blocks.Helpers;
using Cblx.Blocks.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Cblx.Blocks.Factories;

internal class HandlerDeclarationFactory
{
    private readonly ClientGeneratorSettingsBuilder _clientGeneratorPropertiesBuilder;
    private readonly GeneratorExecutionContext _context;

    public HandlerDeclarationFactory(ClientGeneratorSettingsBuilder clientGeneratorPropertiesBuilder, GeneratorExecutionContext context)
    {
        _clientGeneratorPropertiesBuilder = clientGeneratorPropertiesBuilder;
        _context = context;
    }

    public HandlerDeclaration? CreateOrDefault(InterfaceDeclarationSyntax interfaceDeclaration)
    {
        var handlerActionMethod = interfaceDeclaration.IdentifyHandlerActionMethod();

        if (handlerActionMethod is null) return default;

        var handlerAction = HandlerActionDeclarationFactory.CreateOrDefault(handlerActionMethod);

        if(handlerAction is null) return default;


        var name = interfaceDeclaration.Identifier.Text;
        var handlerNamespace = interfaceDeclaration.GetNamespace();
        var clientGeneratorSettings = GetClientGeneratorProperties(interfaceDeclaration);


        return new HandlerDeclaration(name, handlerNamespace, handlerAction,clientGeneratorSettings?.RoutePrefix);
    }

    public ClientGeneratorSettings? GetClientGeneratorProperties(InterfaceDeclarationSyntax interfaceDeclaration)
    {
        var semanticModel = _context.Compilation.GetSemanticModel(interfaceDeclaration.SyntaxTree);

        var interfaceSymbol = semanticModel.GetDeclaredSymbol(interfaceDeclaration, _context.CancellationToken);

        if (interfaceSymbol is not INamedTypeSymbol namedInterfaceSymbol) return null;

        return _clientGeneratorPropertiesBuilder.Build(namedInterfaceSymbol);
    }
    
}
