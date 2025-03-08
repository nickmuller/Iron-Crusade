using Api.Data;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureAppConfiguration((hostContext, config) =>
    {
        config.AddEnvironmentVariables();
        config.AddJsonFile("local.settings.json", true);
    })
    .ConfigureServices((hostContext, services) =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddDbContext<ApiDbContext>(options =>
        {
            var connectionString = hostContext.Configuration.GetConnectionString("SQLConnectionString");
            options.UseSqlServer(connectionString, sqlOptions => sqlOptions.EnableRetryOnFailure());
        });
    })
    .Build();

using (var scope = host.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApiDbContext>();
    await db.Database.MigrateAsync();
}
host.Run();
