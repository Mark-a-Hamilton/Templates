namespace MinAPI.EndPoints;

public static class Endpoint
{
    public static void AddEndpoints(this IEndpointRouteBuilder app)
    {
        #region Variables
        string api = "/api",
            url = "",
            id = "/{Id:int}",
            level = "/{Level}";
        #endregion

        #region Log Table Endpoints
        url = api + "/log";
        static async Task<List<Domain.Models.DmnLog>> GetData(Domain.Models.LogContext context) => await context.Log.ToListAsync();
        
        app.MapGet(url, async (Domain.Models.LogContext context) =>
        {
            return Results.Ok(await GetData(context));
        }).WithName("GetAllLogEntries").WithOpenApi().WithTags(Tags.Log);

        app.MapGet(url + id, async (Domain.Models.LogContext context, int Id) =>
        {
            var item = await context.Log.FindAsync(Id);
            return item == null ? Results.NotFound($"Log Entry with Id: {Id} Not Found") :
                                    Results.Ok(item);
        }).WithName("GetLogById").WithOpenApi().WithTags(Tags.Log);

        app.MapGet(url + level, async (Domain.Models.LogContext context, string Level) =>
        {
            var items = await context.Log.Where(log => log.Level == Level).ToListAsync();
            return (items.Count == 0) ? Results.NotFound($"Log Entry with Level: {Level} Not Found") :
                                        Results.Ok(items);  
        }).WithName("GetLogByLevel").WithOpenApi().WithTags(Tags.Log);

        app.MapPost(url, async (Domain.Models.LogContext context, Domain.Models.DmnLog item) =>
        {
            await context.Log.AddAsync(item);
            await context.SaveChangesAsync();
            return Results.Created();
        }).WithName("PostLogToDatabase").WithOpenApi().WithTags(Tags.Log).AddEndpointFilter<Validation<Domain.Models.DmnLog>>();

        app.MapPut(url + id, async (Domain.Models.LogContext context, Domain.Models.DmnLog item, int Id) =>
        {
            var logItem = await context.Log.FindAsync(Id);
            if (logItem == null) return Results.NotFound($"Unable To Update, Log Entry with Id: {Id} was Not Found");
            logItem.Level = item.Level;
            logItem.Message = item.Message;
            logItem.Exception = item.Exception;
            logItem.Properties = item.Properties;
            context.Log.Update(logItem);
            await context.SaveChangesAsync();
            return Results.Ok();
        }).WithName("PutLogById").WithOpenApi().WithTags(Tags.Log).AddEndpointFilter<Validation<Domain.Models.DmnLog>>();

        app.MapPatch(url + id, async (Domain.Models.LogContext context, Domain.Models.DmnLog item, int Id) =>
        {
            var logItem = await context.Log.FindAsync(Id);
            if (logItem == null) return Results.NotFound($"Unable To Pqrtially Update Log Entry with Id: {Id} was Not Found");
            if (item.Level != null) logItem.Level = item.Level;
            if (item.Message != null) logItem.Message = item.Message;
            if (item.Exception != null) logItem.Exception = item.Exception;
            if (item.Properties != null) logItem.Properties = item.Properties;
            context.Log.Update(logItem);
            await context.SaveChangesAsync();
            return Results.Accepted();
        }).WithName("PatchLogById").WithOpenApi().WithTags(Tags.Log).AddEndpointFilter<Validation<Domain.Models.DmnLog>>();

        app.MapDelete(url + id, async (Domain.Models.LogContext context, int Id) =>
        {
            var item = await context.Log.FindAsync(Id);
            if (item == null) return Results.NotFound($"Unable To Delete Log Entry with Id: {Id} was Not Found");
            context.Remove(item);
            await context.SaveChangesAsync();
            return await context.Log.FindAsync(Id) != null ?
                Results.Problem("Item Could not be deleted") :
                Results.Ok();
        }).WithName("DeleteLogById").WithOpenApi().WithTags(Tags.Log);
        #endregion
    }
}