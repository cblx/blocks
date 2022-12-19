using Cblx.Blocks.Helpers;
using Cblx.Blocks.Models;

namespace Cblx.Blocks.Templates;

internal static class PostVerbMethodBodyTemplete
{
    public static string Create(HandlerDeclaration handler)
    {
        var parameter = handler.HandlerAction.ParameterDeclaration;
        var returnDeclaration = handler.HandlerAction.ReturnDeclaration;
        
        if (parameter is not null && returnDeclaration.HasVoid is false)
        {
            return CreateMethodBodyWithReturnWithParameter(handler);
        }
        
        if (parameter is not null && returnDeclaration.HasVoid)
        {
            return CreateMethodBodyWithoutReturnWithParameter(handler);
        }

        return parameter switch
        {
            null when returnDeclaration.HasVoid is false => CreateMethodBodyWithReturnWithoutParameter(handler),
            null when returnDeclaration.HasVoid => CreateMethodBodyWithoutReturnWithoutParameter(handler),
            _ => InvalidOperationExceptionTemplate.Create("Method declaration is invalid.")
        };
    }

    private static string CreateMethodBodyWithReturnWithParameter(HandlerDeclaration handler)
    {
        var route = StringHelper.CreateEndPointRoute(handler);
        var parameterName = handler.HandlerAction.ParameterDeclaration?.Name;
        var manipulationFormat = handler.HandlerAction.ReturnDeclaration.ManipulationFormat;
        
        return $"""
                var responseMessage = await _httpClient.PostAsJsonAsync("{route}", {parameterName});
                return (await responseMessage.Content.ReadFromJsonAsync<{manipulationFormat}>())!;
        """;
    }
    
    private static string CreateMethodBodyWithoutReturnWithParameter(HandlerDeclaration handler)
    {
        var route = StringHelper.CreateEndPointRoute(handler);
        var parameterName = handler.HandlerAction.ParameterDeclaration?.Name;
        
        return $"""
                _ = await _httpClient.PostAsJsonAsync("{route}", {parameterName});
        """;
    }
    
    private static string CreateMethodBodyWithReturnWithoutParameter(HandlerDeclaration handler)
    {
        var route = StringHelper.CreateEndPointRoute(handler);
        var manipulationFormat = handler.HandlerAction.ReturnDeclaration.ManipulationFormat;
        
        return $"""
                var responseMessage = await _httpClient.PostAsync("{route}", null);
                return (await responseMessage.Content.ReadFromJsonAsync<{manipulationFormat}>())!;
        """;
    }
    
    private static string CreateMethodBodyWithoutReturnWithoutParameter(HandlerDeclaration handler)
    {
        var route = StringHelper.CreateEndPointRoute(handler);
        
        return $"""
                _ = await _httpClient.PostAsync("{route}", null);
        """;
    }
    

}