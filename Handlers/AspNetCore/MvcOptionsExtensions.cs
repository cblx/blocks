using Microsoft.AspNetCore.Mvc;
namespace Cblx.Blocks;

public static class MvcOptionsExtensions
{
    public static MvcOptions PrependHandlerConvention(this MvcOptions options)
    {
        options.Conventions.Add(new HandlerConvention());
        return options;
    }
}
