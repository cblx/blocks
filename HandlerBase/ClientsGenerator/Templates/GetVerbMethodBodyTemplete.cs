using Cblx.Blocks.Helpers;
using Cblx.Blocks.Models;

namespace Cblx.Blocks.Templates;

internal static class GetVerbMethodBodyTemplete
{
    public static string Create(HandlerDeclaration handler)
    {
        if (handler.HandlerAction.ReturnDeclaration.HasVoid)
        {
            return InvalidOperationExceptionTemplate.Create("ReturnDeclaration it can not be VOID.");
        }

        return handler.HandlerAction.ParameterDeclaration is null
            ? CreateMethodBodyWithoutParameter(handler)
            : CreateMethodBodyWithParameter(handler);
    }

    private static string CreateMethodBodyWithoutParameter(HandlerDeclaration handler)
    {
        return $"""
                return (await _httpClient.GetFromJsonAsync<{handler.HandlerAction.ReturnDeclaration.ManipulationType}>($"{StringHelper.CreateEndPointRoute(handler)}"))!;
        """;
    }

    private static string CreateMethodBodyWithParameter(HandlerDeclaration handler)
    {
        return $$"""
                var queryString = QueryStringHelper.ToQueryString({{handler.HandlerAction.ParameterDeclaration?.Name}});
                return (await _httpClient.GetFromJsonAsync<{{handler.HandlerAction.ReturnDeclaration.ManipulationType}}>($"{{StringHelper.CreateEndPointRoute(handler)}}?{queryString}"))!;
        """;
    }
}