using Microsoft.AspNetCore.Routing;
using System.Reflection;
using System.Text;

namespace Cblx.Blocks.RpcEndpoints;

public class EndpointRegistry(IEndpointRouteBuilder endpoints) : IEndpointRegistry
{
    internal static Dictionary<string, EndpointRegistryItem> Items { get; } = [];

    static readonly MethodInfo s_registerMethodInfo = typeof(EndpointRegistry).GetMethods()
                                                .Where(m => m.Name == nameof(Register) && m.GetParameters().Length == 2)
                                                .First(m => m.GetParameters()[0].ParameterType.IsGenericType &&
                                                            m.GetParameters()[0].ParameterType.GetGenericTypeDefinition() == typeof(RpcEndpoint<>));
    
    public IEndpointRegistry Register<TRequest>(RpcEndpoint<TRequest> rpcEndpoint, Delegate executor)
    {
        Items[rpcEndpoint.Path] = new()
        {
            Endpoint = rpcEndpoint,
            Delegate = executor,
            RequestJsonTypeInfo = rpcEndpoint.RequestJsonTypeInfo,
            ResponseJsonTypeInfo = rpcEndpoint.ResponseJsonTypeInfo
        };
        Validate(rpcEndpoint);
        endpoints.MapRpcEndpoint(rpcEndpoint);
        return this;
    }

    public IEndpointRegistry Register<TEndpoint>(TEndpoint endpoint, Func<TEndpoint, Delegate> executorAccessor) where TEndpoint : RpcEndpoint
    {
        var executor = executorAccessor(endpoint);
        var method = s_registerMethodInfo.MakeGenericMethod(endpoint.InternalRequestJsonTypeInfo?.Type ?? typeof(object));
        return (IEndpointRegistry)method.Invoke(this, [endpoint, executor])!;
    }

    public IEndpointRegistry Register<TEndpoint>(Func<TEndpoint, Delegate> executorAccessor) where TEndpoint : RpcEndpoint, new()
    {
        var endpoint = new TEndpoint();
        var executor = executorAccessor(endpoint);
        var method = s_registerMethodInfo.MakeGenericMethod(endpoint.InternalRequestJsonTypeInfo?.Type ?? typeof(object));
        return (IEndpointRegistry)method.Invoke(this, [endpoint, executor])!;
    }

    public IEndpointRegistry Register(Delegate executor)
    {
        var endpoint = (RpcEndpoint)Activator.CreateInstance(executor.Method.DeclaringType!)!;
        var registerMethod = s_registerMethodInfo.MakeGenericMethod(endpoint.InternalRequestJsonTypeInfo?.Type ?? typeof(object));
        return (IEndpointRegistry)registerMethod.Invoke(this, [endpoint, executor])!;
    }

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
