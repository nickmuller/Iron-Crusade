using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Api.Authentication;
using Api.Data;
using IronCrusade.Shared;
using Microsoft.EntityFrameworkCore;

namespace Api;

public class PersoonlijkeGegevensLogFunctions(ILoggerFactory loggerFactory, ApiDbContext db)
{
    private readonly ILogger logger = loggerFactory.CreateLogger<PersoonlijkeGegevensLogFunctions>();
    
    [Function("PersoonlijkeGegevensLogs")]
    public async Task<HttpResponseData> Get([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
    {
        var attribute = GetType().GetMethod(nameof(Get))?.GetCustomAttribute<FunctionAttribute>();
        using var activity = new Activity(attribute?.Name ?? string.Empty).Start(); // Start trace

        var username = StaticWebAppsAuth.Parse(req).Identity!.Name!;
        var persoonlijkeGegevensLogs = await db.PersoonlijkeGegevensLogs.Where(l => l.Username == username)
            .Select(l => new PersoonlijkeGegevensLogModel
            {
                Username = l.Username,
                Datum = l.Datum,
                Gewicht = l.Gewicht
            })
            .OrderBy(l => l.Datum)
            .ToListAsync();

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(persoonlijkeGegevensLogs);

        return response;
    }
    
    [Function("CreatePersoonlijkeGegevensLog")]
    public async Task<HttpResponseData> Post([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
    {
        var attribute = GetType().GetMethod(nameof(Post))?.GetCustomAttribute<FunctionAttribute>();
        using var activity = new Activity(attribute?.Name ?? string.Empty).Start(); // Start trace
        
        var username = StaticWebAppsAuth.Parse(req).Identity!.Name!;
        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var gewicht = JsonSerializer.Deserialize<decimal>(requestBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        db.PersoonlijkeGegevensLogs.Add(new PersoonlijkeGegevensLog
        {
            Username = username,
            Gewicht = gewicht,
            Datum = DateTime.Now
        });
        await db.SaveChangesAsync();

        return req.CreateResponse(HttpStatusCode.Created);
    }
}