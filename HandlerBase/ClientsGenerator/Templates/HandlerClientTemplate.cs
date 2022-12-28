﻿using Cblx.Blocks.Enums;
using Cblx.Blocks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cblx.Blocks.Templates;

internal static class HandlerClientTemplate
{
    public static string Create(HandlerDeclaration handler)
    {
        var builder = new StringBuilder();

        builder.Append($$"""
            // Auto-generated code
            #nullable enable
            using System.Net.Http.Json;
            using System.Diagnostics.CodeAnalysis;
            using Cblx.Blocks;
            {{CreateUsingsIfNotEquals(handler.HandlerAction.ParameterDeclaration?.Namespace, handler.HandlerAction.ReturnDeclaration.Namespace, handler.HandlerNamespace)}}

            namespace {{handler.HandlerNamespace}};

            [ExcludeFromCodeCoverage]
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

    private static string CreateUsingsIfNotEquals(string? namespaceParameter, string? namespaceReturn, string handlerNamespace)
    {
        var listNamespaces = new List<string>
        {
            handlerNamespace
        };

        if (!string.IsNullOrEmpty(namespaceParameter) && !listNamespaces.Contains(namespaceParameter!))
        {
            listNamespaces.Add(namespaceParameter!);
        }

        if (!string.IsNullOrEmpty(namespaceReturn) &&  !listNamespaces.Contains(namespaceReturn!))
        {
            listNamespaces.Add(namespaceReturn!);
        }

        listNamespaces.Remove(handlerNamespace);
        listNamespaces = listNamespaces.Select(n => $"using {n};").ToList();

        return listNamespaces.Any() ? string.Join(Environment.NewLine, listNamespaces) : string.Empty;
    }
}
