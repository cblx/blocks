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

    public HandlerDeclarationFactory(ClientGeneratorSettingsBuilder clientGeneratorPropertiesBuilder)
    {
        _clientGeneratorPropertiesBuilder = clientGeneratorPropertiesBuilder;
    }

    public HandlerDeclaration? CreateOrDefault(InterfaceDeclarationSyntax interfaceDeclaration)
    {
        var handlerActionMethod = interfaceDeclaration.IdentifyHandlerActionMethod();

        if (handlerActionMethod is null) return default;

        var handlerAction = HandlerActionDeclarationFactory.CreateOrDefault(handlerActionMethod);

        if (handlerAction is null) return default;


        var name = interfaceDeclaration.Identifier.Text;
        var handlerNamespace = interfaceDeclaration.GetNamespace();
        var clientGeneratorSettings = GetClientGeneratorProperties(interfaceDeclaration);


        return new HandlerDeclaration(name, handlerNamespace, handlerAction, clientGeneratorSettings?.RoutePrefix);
    }

    private ClientGeneratorSettings? GetClientGeneratorProperties(SyntaxNode interfaceDeclaration)
    {
        var interfaceSymbol = CodeHelpers.GetDeclaredSymbol(interfaceDeclaration);

        return interfaceSymbol is not INamedTypeSymbol namedInterfaceSymbol
            ? null
            : _clientGeneratorPropertiesBuilder.Build(namedInterfaceSymbol);
    }
}