using Microsoft.AspNetCore.Routing;
using System.Linq.Expressions;
using System.Text;

namespace Cblx.Blocks.RpcEndpoints;

public class EndpointRegistry(IEndpointRouteBuilder endpoints) : IEndpointRegistry
{
    internal static Dictionary<string, EndpointRegistryItem> Items { get; } = [];

    [Obsolete]
    public IEndpointRegistry Register<TRequest>(RpcEndpoint<TRequest> rpcEndpoint)
    {
        Items[rpcEndpoint.Path] = new() 
        {
            Delegate = rpcEndpoint.GetDelegate()
        };
        Validate(rpcEndpoint);
        endpoints.MapRpcEndpoint(rpcEndpoint);
        return this;
    }

    public IEndpointRegistry Register<TRequest>(RpcEndpoint<TRequest> rpcEndpoint, Delegate executor)
    {
        Items[rpcEndpoint.Path] = new()
        {
            Delegate = executor
        };
        Validate(rpcEndpoint);
        endpoints.MapRpcEndpoint(rpcEndpoint);
        return this;
    }

    public IEndpointRegistry Register<TEndpoint>(TEndpoint endpoint, Func<TEndpoint, Delegate> executorAccessor) where TEndpoint : RpcEndpoint
    {
        var executor = executorAccessor(endpoint);
        // Chama o método Register que aceita RpcEndpoint<TRequest> e Delegate
        return Register((dynamic)endpoint, executor);
    }

    public IEndpointRegistry Register<TEndpoint>(Func<TEndpoint, Delegate> executorAccessor) where TEndpoint : RpcEndpoint, new()
    {
        var endpoint = new TEndpoint();
        var executor = executorAccessor(endpoint);
        // Chama o método Register que aceita RpcEndpoint<TRequest> e Delegate
        return Register((dynamic)endpoint, executor);
    }

    //public IEndpointRegistry Register<TEndpoint>(TEndpoint endpoint, Func<TEndpoint, Delegate> executorAccessor) where TEndpoint : RpcEndpoint, new()
    //{
    //    var executor = executorAccessor(endpoint);
    //    Items[endpoint.Path] = new()
    //    {
    //        Delegate = executor
    //    };
    //    Validate(endpoint);
    //    endpoints.MapRpcEndpoint(endpoint);
    //    return this;


    //}

    //public IEndpointRegistry Register<TEndpoint>(Expression<Func<TEndpoint, Delegate>> executorAccessor) where TEndpoint : RpcEndpoint, new()
    //{
    //    throw new NotImplementedException();
    //}


    private static void Validate<TRequest>(RpcEndpoint<TRequest> rpcEndpoint)
    {
        var sbError = new StringBuilder();
        var endpointName = rpcEndpoint.GetType().Name;
        var method = Items[rpcEndpoint.Path].Delegate.Method;
        switch (rpcEndpoint)
        {
            case { RequestJsonTypeInfo: null, ResponseJsonTypeInfo: null }: // ActionEndpoint
                if (method.ReturnType != typeof(Task))
                {
                    sbError.AppendLine($"{endpointName} must return a Task.");
                }
                break;
            case { RequestJsonTypeInfo: not null, ResponseJsonTypeInfo: null }: // ActionEndpoint<TRequest>
                if (method.ReturnType != typeof(Task))
                {
                    sbError.AppendLine($"{endpointName} must return a Task.");
                }
                if (method.GetParameters().Length == 0 || method.GetParameters()[0].ParameterType != rpcEndpoint.RequestJsonTypeInfo.Type)
                {
                    sbError.AppendLine($"{endpointName} must have the first parameter of type {rpcEndpoint.RequestJsonTypeInfo.Type.Name}.");
                }
                break;
            case { RequestJsonTypeInfo: not null, ResponseJsonTypeInfo: not null }: // FuncEndpoint<TRequest, TResponse>
                if (!IsTaskOf(method.ReturnType, rpcEndpoint.ResponseJsonTypeInfo.Type))
                {
                    sbError.AppendLine($"{endpointName} must return a Task<{rpcEndpoint.ResponseJsonTypeInfo.Type.Name}>.");
                }
                if (method.GetParameters().Length == 0 || method.GetParameters()[0].ParameterType != rpcEndpoint.RequestJsonTypeInfo.Type)
                {
                    sbError.AppendLine($"{endpointName} must have the first parameter of type {rpcEndpoint.RequestJsonTypeInfo.Type.Name}.");
                }
                break;
            case { RequestJsonTypeInfo: null, ResponseJsonTypeInfo: not null }: // FuncEndpoint<TResponse>
                if (!IsTaskOf(method.ReturnType, rpcEndpoint.ResponseJsonTypeInfo.Type))
                {
                    sbError.AppendLine($"{endpointName} must return a Task<{rpcEndpoint.ResponseJsonTypeInfo.Type.Name}>.");
                }
                break;
            default:
                sbError.AppendLine("Invalid endpoint type.");
                break;
        }
        if (sbError.Length > 0)
        {
            throw new InvalidOperationException(Environment.NewLine + sbError.ToString());
        }
    }


    private static bool IsTaskOf(Type type, Type taskType)
    {
        if (!type.IsGenericType) { return false; }
        if (type.GetGenericTypeDefinition() != typeof(Task<>)) { return false; }
        return type.GetGenericArguments()[0] == taskType;
    }

 
}
