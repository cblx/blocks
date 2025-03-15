namespace Cblx.Blocks.RpcEndpoints;

internal class ValidationFilter<TRequest>(IValidator<TRequest> validator) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var argumentToValidate = context.GetArgument<TRequest>(0);
        if (argumentToValidate is null)
        {
            return Results.BadRequest($"Request is null or not of type {nameof(TRequest)}");
        }

        var validationResult = await validator.ValidateAsync(argumentToValidate);

        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary(), statusCode: (int)HttpStatusCode.UnprocessableEntity);
        }

        // Otherwise invoke the next filter in the pipeline
        return await next.Invoke(context);
    }
}
#pragma warning restore CS0067
