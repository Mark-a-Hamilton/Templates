namespace Domain.Validators;

public class Validation<T> : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var vldtr = context.HttpContext.RequestServices.GetRequiredService<IValidator<T>>();
        if (vldtr != null)
        {
            var item = context.Arguments.OfType<T>().FirstOrDefault(a => a?.GetType() == typeof(T));
            if (item != null)
            {
                var result = await vldtr.ValidateAsync(item);
                if (!result.IsValid) return Results.ValidationProblem(result.ToDictionary());
            }
            else return Results.Problem("Could Not Find type to validate!!!");
        }
        return await next(context);
    }
}