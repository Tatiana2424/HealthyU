using Asp.Versioning.ApiExplorer;
using Asp.Versioning;
using BarberShop.WebApi.Extensions;
using HealthuU.BLL.Model;
using HealthuU.BLL.Services.Interfaces;
using HealthuU.BLL.Services.Realizations;

using HealthyU.WebApi.Configurations;
using HealthyU.WebApi.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;


using System.Text;
using Microsoft.OpenApi.Models;
using System.Reflection;
using HealthyU.WebApi.Middlewares;
using HealthyU.WebApi.Extensions;
using HealthuU.BLL.Services.Interfaces.Encryption;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.OutputCaching;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<OpenAI>(builder.Configuration.GetSection("OpenAI"));

var jwtSettingsSection = builder.Configuration.GetSection("Jwt");

builder.Services.Configure<JwtSettings>(jwtSettingsSection);

var jwtSettings = jwtSettingsSection.Get<JwtSettings>();
var key = Encoding.ASCII.GetBytes(jwtSettings.Key);

builder.Services.Configure<AesSettings>(
    builder.Configuration.GetSection("AesSettings")
);
builder.Services.Configure<RsaSettings>(
    builder.Configuration.GetSection("RsaSettings")
);

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();
builder.Services
  .AddApiVersioning(options =>
  {
      options.DefaultApiVersion = new ApiVersion(1, 0);
      options.AssumeDefaultVersionWhenUnspecified = true;
      options.ReportApiVersions = true;
  })
  .AddApiExplorer(options =>
  {
      options.GroupNameFormat = "'v'VVV";
      options.SubstituteApiVersionInUrl = true;
  });

builder.Services.AddSwaggerGen(c =>
{
    var provider = builder.Services.BuildServiceProvider()
                      .GetRequiredService<IApiVersionDescriptionProvider>();

    foreach (var desc in provider.ApiVersionDescriptions)
    {
        c.SwaggerDoc(desc.GroupName, new OpenApiInfo
        {
            Title = $"HealthyU API {desc.ApiVersion}",
            Version = desc.GroupName,
            Description = desc.IsDeprecated
                ? "This API version is deprecated."
                : "RESTful API for HealthyU",
            Contact = new OpenApiContact
            {
                Name = "HealthyU Team",
                Email = "support@healthyu.com"
            },
            License = new OpenApiLicense
            {
                Name = "MIT",
                Url = new Uri("https://opensource.org/licenses/MIT")
            }
        });
    }

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header. Enter only your token.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddCustomServices();
builder.Services.ConfigureServices(builder.Configuration);
builder.Services.AddHttpClient();

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

}).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
.AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
{
    options.ClientId = builder.Configuration.GetSection("GoogleKeys:ClientId").Value;
    options.ClientSecret = builder.Configuration.GetSection("GoogleKeys:ClientSecret").Value;
    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.CallbackPath = "/signin-google";
    options.SaveTokens = true;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Key)),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtSettings.Audience,
        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.AddControllers();

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddSingleton(resolver =>
    resolver.GetRequiredService<IOptions<JwtSettings>>().Value);

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IOpenAIService, OpenAIService>();
builder.Services.AddScoped<LogExecutionFilter>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<LogExecutionFilter>();
});
builder.Services.AddMemoryCache();
builder.Services.AddOutputCache(options =>
{
    options.AddBasePolicy(builder =>
    {
        builder.Expire(TimeSpan.FromSeconds(10));
    });

    options.AddPolicy("Expire20", builder =>
    {
        builder.Expire(TimeSpan.FromSeconds(20));
    });


    options.AddPolicy("Expire30", builder =>
    {
        builder.Expire(TimeSpan.FromSeconds(30));
    });
});

var app = builder.Build();
await app.Services.EnsureRolesAndAdminAsync();
using var scope = app.Services.CreateScope();
var keyProv = scope.ServiceProvider.GetRequiredService<IRsaKeyProvider>();
keyProv.GenerateAndSaveKeyPair();

app.UseMiddleware<ErrorHandlingMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        foreach (var desc in provider.ApiVersionDescriptions)
        {
            c.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json",
                              desc.GroupName.ToUpperInvariant());
        }
    });
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseOutputCache();
app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();
