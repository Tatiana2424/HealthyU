using Microsoft.EntityFrameworkCore;

namespace HealthyU.WebApi.Extensions;

public static class WebAppExtensions
{
    public static async Task MigrateAndSeedDbAsync(
        this WebApplication app
      )
    {
        using var scope = app.Services.CreateScope();
        try
        {
            var healthyUDbContext = scope.ServiceProvider.GetRequiredService<HealthyUDbContext>();
            var projRootDirectory = Directory.GetParent(Environment.CurrentDirectory)?.FullName!;

            var scriptFiles = Directory.GetFiles($"{projRootDirectory}");

            await healthyUDbContext.Database.EnsureDeletedAsync();
            await healthyUDbContext.Database.MigrateAsync();

            var filesContexts = await Task.WhenAll(scriptFiles.Select(file => File.ReadAllTextAsync(file)));

            foreach (var task in filesContexts)
            {
                await healthyUDbContext.Database.ExecuteSqlRawAsync(task);
            }
        }
        catch (Exception ex)
        {
            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occured during startup migration");
        }
    }
}