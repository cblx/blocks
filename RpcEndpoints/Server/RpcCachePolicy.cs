using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Primitives;

namespace Cblx.Blocks.RpcEndpoints;

/// <summary>
/// Policy abaixo serve para termos Cache nos endpoints
/// mesmo que sejam endpoints que necessitam de autorização.
/// Ele é o DefaultPolicy do Asp.NET mas retirando a verificação
/// que checa se existe cabeçalho de autorização.
/// Ideia baseada neste post no github: https://github.com/dotnet/AspNetCore.Docs/issues/6836#issuecomment-1502530163
/// Código do DefaultPolicy: https://github.com/dotnet/aspnetcore/blob/42af9fe6ddd7c3f9cde04ac003bf97509881873b/src/Middleware/OutputCaching/src/Policies/DefaultPolicy.cs#L21
/// </summary>
public class RpcCachePolicy : IOutputCachePolicy
{
    public static readonly RpcCachePolicy Instance = new();
    /// <inheritdoc />
    ValueTask IOutputCachePolicy.CacheRequestAsync(OutputCacheContext context, CancellationToken cancellationToken)
    {
        context.EnableOutputCaching = true;
        context.AllowCacheLookup = true;
        context.AllowCacheStorage = true;
        context.AllowLocking = true;
        // Vary by any query by default
        context.CacheVaryByRules.QueryKeys = "*";
        VaryByBody(context);
        return ValueTask.CompletedTask;
    }

    // Based on https://stackoverflow.com/a/77963818/1851755
    private static void VaryByBody(OutputCacheContext context)
    {
        context.HttpContext.Request.EnableBuffering();

        using var reader = new StreamReader(context.HttpContext.Request.Body, leaveOpen: true);
        var body = reader.ReadToEndAsync();

        // Reset the stream position to enable subsequent reads
        context.HttpContext.Request.Body.Position = 0;

        var keyVal = new KeyValuePair<string, string>("requestBody", body.Result);
        context.CacheVaryByRules.VaryByValues.Add(keyVal);
    }

    /// <inheritdoc />
    ValueTask IOutputCachePolicy.ServeFromCacheAsync(OutputCacheContext context, CancellationToken cancellationToken)
    {
        return ValueTask.CompletedTask;
    }

    /// <inheritdoc />
    ValueTask IOutputCachePolicy.ServeResponseAsync(OutputCacheContext context, CancellationToken cancellationToken)
    {
        var response = context.HttpContext.Response;
        // Verify existence of cookie headers
        if (!StringValues.IsNullOrEmpty(response.Headers.SetCookie))
        {
            context.AllowCacheStorage = false;
            return ValueTask.CompletedTask;
        }
        // Check response code
        if (response.StatusCode != StatusCodes.Status200OK)
        {
            context.AllowCacheStorage = false;
            return ValueTask.CompletedTask;
        }
        return ValueTask.CompletedTask;
    }
}
