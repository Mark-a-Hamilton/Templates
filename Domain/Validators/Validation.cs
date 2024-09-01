namespace Domain.Validators;

/// <summary>
/// A generic validation filter for endpoint requests.
/// </summary>
/// <typeparam name="T">The type of the object to validate.</typeparam>
public class Validation<T> : IEndpointFilter
{
    /// <summary>
    /// Invokes the validation filter asynchronously.
    /// </summary>
    /// <param name="context">The context of the endpoint filter invocation.</param>
    /// <param name="next">The next delegate to invoke in the filter pipeline.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the response object.</returns>
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        // Retrieve the validator service for the specified type T.
        var vldtr = context.HttpContext.RequestServices.GetRequiredService<IValidator<T>>();
        if (vldtr != null)
        {
            // Find the first argument of type T.
            var item = context.Arguments.OfType<T>().FirstOrDefault(a => a?.GetType() == typeof(T));
            if (item != null)
            {
                var result = await vldtr.ValidateAsync(item);   // Validate the item.
                // If validation fails, return a validation problem result.
                if (!result.IsValid) return Results.ValidationProblem(result.ToDictionary());
            }
            else
                // Return a problem result if the item to validate is not found.
                return Results.Problem("Could Not Find type to validate!!!");
        }
        return await next(context);     // Proceed to the next filter or endpoint handler.
    }
}