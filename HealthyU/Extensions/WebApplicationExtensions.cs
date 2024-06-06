//using BarberShop.DAL.Persistence;
//using Microsoft.EntityFrameworkCore;


//namespace Streetcode.WebApi.Extensions;

//public static class WebApplicationExtensions
//{
//    public static async Task MigrateAndSeedDbAsync(
//        this WebApplication app
//      )
//    {
//        using var scope = app.Services.CreateScope();
//        try
//        {
//            var barberShopDbContext = scope.ServiceProvider.GetRequiredService<BarberShopDbContext>();
//            var projRootDirectory = Directory.GetParent(Environment.CurrentDirectory)?.FullName!;

//            var scriptFiles = Directory.GetFiles($"{projRootDirectory}");

//            await barberShopDbContext.Database.EnsureDeletedAsync();
//            await barberShopDbContext.Database.MigrateAsync();

//            var filesContexts = await Task.WhenAll(scriptFiles.Select(file => File.ReadAllTextAsync(file)));

//            foreach (var task in filesContexts)
//            {
//                await barberShopDbContext.Database.ExecuteSqlRawAsync(task);
//            }
//        }
//        catch (Exception ex)
//        {
//            var logger = app.Services.GetRequiredService<ILogger<Program>>();
//            logger.LogError(ex, "An error occured during startup migration");
//        }
//    }
//}