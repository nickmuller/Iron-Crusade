using System.Diagnostics;
using System.Net;
using System.Reflection;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Api.Authentication;
using Api.Data;
using IronCrusade.Shared;
using Microsoft.EntityFrameworkCore;

namespace Api;

public class WorkoutLogFunctions(ILoggerFactory loggerFactory, ApiDbContext db)
{
    private readonly ILogger logger = loggerFactory.CreateLogger<WorkoutLogFunctions>();
    
    [Function("WorkoutLogs")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
    {
        var attribute = GetType().GetMethod(nameof(Run))?.GetCustomAttribute<FunctionAttribute>();
        using var activity = new Activity(attribute?.Name ?? string.Empty).Start(); // Start trace

        var username = StaticWebAppsAuth.Parse(req).Identity!.Name;
        var workoutLogs = await db.WorkoutLogs.Where(l => l.Username == username)
            .Select(l => new WorkoutLogModel
            {
                Username = l.Username,
                Categorie = l.Categorie,
                WorkoutStart = l.WorkoutStart,
                WorkoutEind = l.WorkoutEind
            })
            .OrderBy(l => l.WorkoutStart)
            .ToListAsync();

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(workoutLogs);

        return response;
    }
}