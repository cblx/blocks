using Cblx.Blocks.Enums;
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
                public {{handler.ImplementationName}}Client(HttpClient httpClient) => _httpClient = httpClient;

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
            case HttpVerb.Get: builder.CreateGetMethodBody(handler); break;
            case HttpVerb.Post: CreatePostMethodBody(builder, handler); break;
            case HttpVerb.Delete: CreateDeleteMethodBody(builder, handler); break;
            case HttpVerb.Unknown: builder.AppendLine(InvalidOperationExceptionTemplate.Create("Unknown verb")); break;
            default: builder.AppendLine("\t\t\t // Not Identifier"); break;
        }
    }

    private static void CreateGetMethodBody(this StringBuilder builder, HandlerDeclaration handler) 
        => builder.AppendLine(GetVerbMethodBodyTemplete.Create(handler));

    private static void CreatePostMethodBody(StringBuilder builder, HandlerDeclaration handler) 
        => builder.AppendLine(PostVerbMethodBodyTemplete.Create(handler));
    
    private static void CreateDeleteMethodBody(StringBuilder builder, HandlerDeclaration handler) 
        => builder.AppendLine(DeleteVerbMethodBodyTemplete.Create(handler));
}
