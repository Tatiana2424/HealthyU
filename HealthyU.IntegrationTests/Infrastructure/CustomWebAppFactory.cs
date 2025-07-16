using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using Microsoft.AspNetCore.Mvc.Testing;

public class CustomWebAppFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<HealthyUDbContext>));
            if (descriptor != null) services.Remove(descriptor);

            services.AddDbContext<HealthyUDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
            });

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<HealthyUDbContext>();
            db.Database.EnsureCreated();

            SeedTestData(db);
        });
    }

    private void SeedTestData(HealthyUDbContext db)
    {
        if (!db.Articles.Any())
        {
            db.Articles.Add(
                new HealthyU.DAL.Entities.Article 
                { 
                    Id = 1, 
                    Title = "Test", 
                    IsPublished = true,
                    Description = "Test",
                    ImageId = 2
                });
            db.Images.Add(
                new HealthyU.DAL.Entities.Image
                {
                    Id = 2,
                    Title = "Test",
                    Url = "/image",
                    Alt = "alt"
                });
            db.SaveChanges();
        }
    }
}
