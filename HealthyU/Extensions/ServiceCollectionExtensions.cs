
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using HealthyU.DAL.Repositories.Interfaces.Base;
using HealthyU.DAL.Repositories.Realizations;
using HealthyU.DAL.Repositories.Realizations.Base;
using HealthyU.DAL.Repositories.Interfaces;
using HealthuU.BLL.Services.Interfaces;
using HealthuU.BLL.Services.Interfaces.Logging;
using HealthuU.BLL.Services.Realizations.Logging;
using HealthuU.BLL.Services.Realizations;
using HealthyU.DAL.Entities;
using HealthuU.BLL.Services.Interfaces.Cache;
using HealthuU.BLL.Services.Realizations.Cache;

namespace BarberShop.WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddRepositoryServices(this IServiceCollection services)
    {
        services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRecipeRepository, RecipeRepository>();
        services.AddScoped<IArticleRepository, ArticleRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IImageRepository, ImageRepository>();
        services.AddScoped<ISearchKeywordRepository, SearchKeywordRepository>();
    }
    public static void AddCustomServices(this IServiceCollection services)
    {
        services.AddRepositoryServices();
        
        var currentAssemblies = AppDomain.CurrentDomain.GetAssemblies();
        services.AddAutoMapper(currentAssemblies);

        services.AddScoped<ISearchKeywordService, SearchKeywordService>();
        services.AddScoped<IImageService, ImageService>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IArticleService, ArticleService>();
        services.AddScoped<IRecipeService, RecipeService>();
        services.AddScoped<HealthuU.BLL.Services.Interfaces.IAuthenticationService, HealthuU.BLL.Services.Realizations.AuthenticationService>();
        //services.AddSingleton<IRsaKeyProvider, RsaKeyProvider>();
        //services.AddScoped<IAsymmetricEncryptionService, RsaEncryptionService>();
        //services.AddScoped<IHybridEncryptionService, HybridEncryptionService>();
        //services.AddScoped<IAesEncryptionService, AesEncryptionService>();
        services.AddScoped<IRecipeImportService, RecipeImportService>();
        services.AddSingleton<ICacheService, MemoryCacheService>();
        services.AddScoped<IBmiService, BmiService>();
        //services.AddTransient(typeof(ILoggerService<>), typeof(LoggerService<>));
        services.AddScoped(typeof(ILoggerService<>), typeof(LoggerService<>));
        //services.AddSingleton(typeof(ILoggerService<>), typeof(LoggerService<>));
    }
    public static void AddApplicationServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddDbContext<HealthyUDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("HealthyUDB"), opt =>
            {
                opt.MigrationsAssembly(typeof(HealthyUDbContext).Assembly.GetName().Name);
                opt.MigrationsHistoryTable("__EFMigrationsHistory", schema: "entity_framework");
            });
        });

        services.AddIdentity<User, IdentityRole<int>>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 6;
        })
        .AddEntityFrameworkStores<HealthyUDbContext>()
        .AddDefaultTokenProviders();


        services.AddCors(opt =>
        {
            opt.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyOrigin();
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.SetPreflightMaxAge(TimeSpan.FromDays(1));
            });
        });

        services.AddControllers();
    }

    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        //    .AddCookie(options =>
        //    {
        //        options.Cookie.Name = "MyAppCookie";
        //        options.LoginPath = "/Account/Login";
        //        options.AccessDeniedPath = "/Account/Forbidden";
        //    });
        //services.Configure<CookiePolicyOptions>(options =>
        //{
        //    options.MinimumSameSitePolicy = SameSiteMode.Strict;
        //    options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always;
        //    options.Secure = CookieSecurePolicy.SameAsRequest;      
        //});

        //services.Configure<CookieAuthenticationOptions>(options =>
        //{
        //    options.Cookie.Name = configuration.GetValue<string>("CookieSettings:CookieName");
        //    options.ExpireTimeSpan = TimeSpan.FromMinutes(configuration.GetValue<int>("CookieSettings:ExpirationTimeInMinutes"));
        //    options.SlidingExpiration = configuration.GetValue<bool>("CookieSettings:SlidingExpiration");
        //});

        //services.Configure<IdentityOptions>(options =>
        //{
        //    options.ClaimsIdentity.UserIdClaimType = "user_id";
        //});

        //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        //.AddCookie(options =>
        //{
        //    options.Events.OnValidatePrincipal = async context =>
        //    {
        //        var expiresUtc = context.Properties.GetTokenValue("expires_at");
        //        if (DateTimeOffset.TryParse(expiresUtc, out var expires) &&
        //            expires < DateTimeOffset.UtcNow.AddMinutes(10))
        //        {
        //            var refreshToken = context.Properties.GetTokenValue("refresh_token");
        //            var newToken = await new TokenService().RefreshToken(refreshToken); // Reference TokenService
        //            context.Properties.UpdateTokenValue("access_token", newToken);
        //            context.Properties.UpdateTokenValue("expires_at", GetExpirationTime().ToString("o"));
        //            context.ShouldRenew = true;
        //        }
        //    };
        //});
    }

}
