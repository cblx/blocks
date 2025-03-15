using Microsoft.AspNetCore.Routing;
using System.Text;

namespace Cblx.RpcEndpoints;

public class EndpointRegistry(IEndpointRouteBuilder endpoints) : IEndpointRegistry
{
    public IEndpointRegistry Register<TRequest>(RpcEndpoint<TRequest> rpcEndpoint)
    {
        Validate(rpcEndpoint);
        endpoints.MapRpcEndpoint(rpcEndpoint);
        return this;
    }
    private static void Validate<TRequest>(RpcEndpoint<TRequest> rpcEndpoint)
    {
        var sbError = new StringBuilder();
        var endpointName = rpcEndpoint.GetType().Name;
        var method = rpcEndpoint.GetDelegate().Method;
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
                if (method.GetParameters().Length == 0 || method.GetParameters()[0].ParameterType != typeof(TRequest))
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
