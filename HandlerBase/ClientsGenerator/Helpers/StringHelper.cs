using Cblx.Blocks.Models;

namespace Cblx.Blocks.Helpers;

internal static class StringHelper
{
    public static string CreateEndPointRoute(HandlerDeclaration handler)
    {
        return handler.RoutePrefix is null
            ? handler.ImplementationName
            : $"{handler.RoutePrefix}/{handler.ImplementationName}";
    }
}