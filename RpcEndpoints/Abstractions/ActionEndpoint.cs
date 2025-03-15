namespace Cblx.Blocks.RpcEndpoints;

public abstract class ActionEndpoint(bool allowAnonymous = false) : RpcEndpoint<object>(null,
                                                                                        null,
                                                                                        null,
                                                                                        allowAnonymous,
                                                                                        cache: null);
public abstract class ActionEndpoint<TRequest>(JsonTypeInfo<TRequest> requestTypeInfo,
                                               IValidator<TRequest>? validator = null,
                                               bool allowAnonymous = false) : RpcEndpoint<TRequest>(requestTypeInfo,
                                                                                                    null,
                                                                                                    validator,
                                                                                                    allowAnonymous,
                                                                                                    cache: null);