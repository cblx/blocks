namespace Cblx.Blocks.RpcEndpoints;

public abstract class FuncEndpoint<TResponse>(
    JsonTypeInfo<TResponse> responseTypeInfo,
    bool allowAnonymous = false,
    TimeSpan? cache = null,
    bool disableAntiforgery = false) : RpcEndpoint<object>(null,
                                                        responseTypeInfo,
                                                        null,
                                                        allowAnonymous,
                                                        cache,
                                                        disableAntiforgery);
public abstract class FuncEndpoint<TRequest, TResponse>(
    JsonTypeInfo<TRequest> requestTypeInfo,
    JsonTypeInfo<TResponse> responseTypeInfo,
    IValidator<TRequest>? validator = null,
    bool allowAnonymous = false,
    TimeSpan? cache = null,
    bool disableAntiforgery = false) : RpcEndpoint<TRequest>(requestTypeInfo,
                            responseTypeInfo,
                            validator,
                            allowAnonymous,
                            cache,
                            disableAntiforgery);