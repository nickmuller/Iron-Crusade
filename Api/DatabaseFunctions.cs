using System.Diagnostics;
using System.Net;
using System.Reflection;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Api.Data;

namespace Api;

public class DatabaseFunctions(ApiDbContext db)
{
    [Function("CanConnectToDatabase")]
    public async Task<HttpResponseData> Get([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
    {
        var attribute = GetType().GetMethod(nameof(Get))?.GetCustomAttribute<FunctionAttribute>();
        using var activity = new Activity(attribute?.Name ?? string.Empty).Start(); // Start trace

        var canConnect = await db.Database.CanConnectAsync();
        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(canConnect);

        return response;
    }
}