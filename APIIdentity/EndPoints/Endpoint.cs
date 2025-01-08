namespace APIIdentity.EndPoints;

    public static class Endpoint
    {
        public static void AddEndpoints(this IEndpointRouteBuilder app)
        {
            #region Define Variables
            var api = app.MapGroup(Tags.Api); // Adds prefix to all endpoints that use api variable.
            var Example = api.MapGroup(Tags.Example.ToLower()).WithTags(Tags.Example); // adds the tablename to the endpoint and groups them
            #endregion Define Variables

            #region Endpoints
            #region Example
            Example.MapGet("eg", () => "APIIdentity - Fancy that, it works!!!!");
            #endregion Example
            #endregion Endpoints
        }
    }
