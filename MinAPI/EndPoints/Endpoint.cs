namespace MinAPI.EndPoints;

public static class Endpoint
{
    public static void AddEndpoints(this IEndpointRouteBuilder app)
    {
        #region Define Variables
        string example = "Hello, World !!!!";
        #endregion

        #region Example Endpoint
        app.MapGet("/", () => example);
        #endregion


    }
}