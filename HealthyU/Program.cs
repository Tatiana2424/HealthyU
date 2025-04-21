using BarberShop.WebApi.Extensions;
using HealthuU.BLL.Model;
using HealthuU.BLL.Services.Interfaces;
using HealthuU.BLL.Services.Realizations;

using HealthyU.WebApi.Configurations;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<OpenAI>(builder.Configuration.GetSection("OpenAI"));

//builder.Services.AddRazorPages();

//var provider = builder.Services.BuildServiceProvider();
//var configuration = provider.GetService<IConfiguration>();

var jwtSettingsSection = builder.Configuration.GetSection("Jwt");
//builder.WebHost.ConfigureKestrel(serverOptions =>
//{
//    serverOptions.ListenAnyIP(80);
//});

builder.Services.Configure<JwtSettings>(jwtSettingsSection);

var jwtSettings = jwtSettingsSection.Get<JwtSettings>();
var key = Encoding.ASCII.GetBytes(jwtSettings.Key);

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCustomServices();
builder.Services.ConfigureServices(builder.Configuration);
builder.Services.AddHttpClient();

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false; // For development, in production set to true
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
        // Set ValidateIssuer and ValidateAudience to true and set valid Issuer and Audience if needed.
    };
});
builder.Services.AddControllers();
//builder.Services.AddCors(options =>
//{
//    var frontendURL = configuration.GetValue<string>("frontend_Url");

//    options.AddDefaultPolicy(builder =>
//    {
//        builder.WithOrigins(frontendURL).AllowAnyMethod().AllowAnyHeader();
//    });
//});
// Bind JwtSettings from appsettings.json
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Register JwtSettings for DI
builder.Services.AddSingleton(resolver =>
    resolver.GetRequiredService<IOptions<JwtSettings>>().Value);

// Add services to the container.
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IOpenAIService, OpenAIService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseCors();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();
