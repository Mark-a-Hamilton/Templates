namespace MinAPI.EndPoints;

public static class Endpoint
{
    public static void AddEndpoints(this IEndpointRouteBuilder app)
    {
        #region Define Variable
        var api = app.MapGroup("api"); // Adds prefix to all endpoints that use variable.
        var Example = api.MapGroup("Example").WithTags(Tags.Example); // adds the tablename to the endpoint and puts them in the Endpoint group that use variable 
        #endregion

        #region Example Endpoint
        Example.MapGet("eg", () => "Fancy that, it works!!!!");
        #endregion
    }
}