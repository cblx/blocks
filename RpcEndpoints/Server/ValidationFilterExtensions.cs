namespace Cblx.Blocks.RpcEndpoints;

internal static class ValidationFilterExtensions
{
    public static RouteHandlerBuilder AddValidator<TRequest>(this RouteHandlerBuilder builder, IValidator<TRequest> validator)
    {
        return builder.AddEndpointFilter(new ValidationFilter<TRequest>(validator));
    }
}
