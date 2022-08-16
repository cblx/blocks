using Cblx.Blocks.Enums;
using Cblx.Blocks.Factories;
using Cblx.Blocks.Helpers;
using Cblx.Blocks.Models;
using System.Text;

namespace Cblx.Blocks.Templates;

internal static class HandlerClientTemplate
{
    public static string Create(HandlerDeclaration handler)
    {
        var builder = new StringBuilder();

        builder.Append($$"""
            #nullable enable
            using System.Net.Http.Json;
            using Cblx.Blocks;

            namespace {{handler.HandlerNamespace}};

            public class {{handler.ImplementationName}}Client : {{handler.InterfaceName}}
            {
                private readonly HttpClient _httpClient;
                public {{handler.ImplementationName}}Client(HttpClient httpClient)
                {
                    _httpClient = httpClient;
                }

                public {{handler.CreateAsyncToken()}} {{handler.HandlerAction.ReturnDeclaration.MethodReturnFormat}} {{handler.HandlerAction.Name}}({{handler.HandlerAction.ParameterDeclaration?.MethodParameterFormat}})
                {

            """);

        CreateMethodBody(builder, handler);

        builder.Append("""
                }
            }
            """);

        return builder.ToString();
    }

    private static void CreateMethodBody(StringBuilder builder, HandlerDeclaration handler)
    {
        switch (handler.HandlerAction.Verb)
        {
            case HttpVerb.Get: CreateGetMethodBody(builder, handler); break;
            case HttpVerb.Post: CreatePostMethodBody(builder, handler); break;
            case HttpVerb.Delete: CreateDeleteMethodBody(builder, handler); break;

            case HttpVerb.Unknown:
            default: builder.AppendLine("\t\t\t // Not Identifier"); break;
        }
    }

    private static void CreateGetMethodBody(StringBuilder builder, HandlerDeclaration handler)
    {

        if (handler.HandlerAction.ParameterDeclaration is null)
        {
            builder.AppendLine($"""
                        return (await _httpClient.GetFromJsonAsync<{handler.HandlerAction.ReturnDeclaration.ManipulationFormat}>($"{StringHelper.CreateEndPointRoute(handler)}"))!; 
                """);

            return;
        }

        builder.AppendLine($$"""
                    var queryString = QueryStringHelper.ToQueryString({{handler.HandlerAction.ParameterDeclaration.Name}});
                    return (await _httpClient.GetFromJsonAsync<{{handler.HandlerAction.ReturnDeclaration.ManipulationFormat}}>($"{{StringHelper.CreateEndPointRoute(handler)}}?{queryString}"))!;
            """);
    }

    private static void CreatePostMethodBody(StringBuilder builder, HandlerDeclaration handler)
    {
        if (handler.HandlerAction.ParameterDeclaration is null) return;        

        builder.AppendLine($"""
                    var responseMessage = await _httpClient.PostAsJsonAsync("{StringHelper.CreateEndPointRoute(handler)}", {handler.HandlerAction.ParameterDeclaration.Name});
            """
        );

        if (!handler.HandlerAction.ReturnDeclaration.HasVoid)
        {
            builder.AppendLine($"""
                        return (await responseMessage.Content.ReadFromJsonAsync<{handler.HandlerAction.ReturnDeclaration.ManipulationFormat}>())!;
                """
            );
        }
    }

    private static void CreateDeleteMethodBody(StringBuilder builder, HandlerDeclaration handler)
    {
        if (handler.HandlerAction.ParameterDeclaration is null) return;        

        builder.AppendLine($$"""
                    var queryString = QueryStringHelper.ToQueryString({handler.HandlerAction.ParameterDeclaration.Name});
                    var responseMessage = await _httpClient.DeleteAsync($"{{StringHelper.CreateEndPointRoute(handler)}}?{queryString}");
            """
        );

        if (!handler.HandlerAction.ReturnDeclaration.HasVoid)
        {
            builder.AppendLine($"""
                        return (await responseMessage.Content.ReadFromJsonAsync<{handler.HandlerAction.ReturnDeclaration.ManipulationFormat}>())!;
                """
            );
        }
    }


}
