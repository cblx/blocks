namespace Cblx.Blocks.RpcEndpoints;

public abstract class ActionEndpoint(bool allowAnonymous = false, bool disableAntiforgery = false) : RpcEndpoint<object>(null,
                                                                                        null,
                                                                                        null,
                                                                                        allowAnonymous,
                                                                                        cache: null,
                                                                                        disableAntiforgery: disableAntiforgery);
public abstract class ActionEndpoint<TRequest>(JsonTypeInfo<TRequest> requestTypeInfo,
                                               IValidator<TRequest>? validator = null,
                                               bool allowAnonymous = false,
                                               bool disableAntiforgery = false) : RpcEndpoint<TRequest>(requestTypeInfo,
                                                                                                    null,
                                                                                                    validator,
                                                                                                    allowAnonymous,
                                                                                                    cache: null,
                                                                                                    disableAntiforgery: disableAntiforgery);