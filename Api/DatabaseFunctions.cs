using Microsoft.Azure.Functions.Worker;
using Api.Data;
using Microsoft.Extensions.Logging;

namespace Api;

public class DatabaseFunctions(ApiDbContext db)
{
    [Function(nameof(CanConnectToDatabase))]
    public async Task CanConnectToDatabase([TimerTrigger("0 0 12 * * mon,fri")] TimerInfo myTimer, FunctionContext context)
    {
        var logger = context.GetLogger(nameof(CanConnectToDatabase));
        var canConnect = await db.Database.CanConnectAsync();
        logger.LogInformation("Can connect to the database: {CanConnect}", canConnect);
    }
}