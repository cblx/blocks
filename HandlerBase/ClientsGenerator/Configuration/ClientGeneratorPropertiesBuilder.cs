using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Linq;
using Cblx.Blocks.Extensions;
using System.Collections.Generic;

namespace Cblx.Blocks.Configuration;

internal sealed class ClientGeneratorSettingsBuilder
{
    private readonly ImmutableArray<KeyValuePair<string, TypedConstant>>? _globalParameters;
    public ClientGeneratorSettingsBuilder(IAssemblySymbol context)
    {
        _globalParameters = GetClientGeneratorConfigurationOrDefault(context);
    }

    public ClientGeneratorSettings? Build(ISymbol builderSymbol)
    {
        if ((GetClientGeneratorConfigurationOrDefault(builderSymbol) ?? _globalParameters) is not { } attributeParameters)
            return null;

        var prefixRoute = attributeParameters.GetOrDefault<string>(nameof(ClientGeneratorSettings.RoutePrefix));

        return new ClientGeneratorSettings(prefixRoute);
    }

    private static ImmutableArray<KeyValuePair<string, TypedConstant>>? GetClientGeneratorConfigurationOrDefault(ISymbol context)
    {
        var attributesData = context.GetAttributes();
        var attribute = attributesData.SingleOrDefault(x => x.AttributeClass.HasNameOrBaseClassHas(nameof(GenerateClientAttribute)));
        
        if (attribute is null) return null;
        return attribute.NamedArguments.Length <= 0 ? null : attribute.NamedArguments;
    }
}
