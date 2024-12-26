namespace API.EndPoints;

public static class Endpoint
{
    public static void AddEndpoints(this IEndpointRouteBuilder app)
    {
        #region Define Variable
        var api = app.MapGroup(Tags.Api); // Adds prefix to all endpoints that use api variable.
        var Example = api.MapGroup(Tags.Example.ToLower()).WithTags(Tags.Example); // adds the tablename to the endpoint and groups them
        #endregion

        Example.MapGet("eg", () => "Fancy that, it works!!!!");     // Example Endpoint
    }
} 