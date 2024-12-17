using Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Web;
using Web.Configurations;

var builder = WebApplication.CreateBuilder(args);
var jwtKey = builder.Configuration["Jwt:Key"];

ServiceConfigurator.ConfigureServices(builder.Services);

builder.Services.AddControllers();
builder.Services.ConfigureJwtAuthentication(jwtKey);
builder.Services.ConfigureSwaggerWithJwt();
builder.Services.AddScoped<IPasswordHasher<Data.Entities.User>, PasswordHasher<Data.Entities.User>>();
builder.Services.AddDbContext<Repository>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure the application to listen on all interfaces
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080); // Listen on port 8080 for all network interfaces
});

// Disable HTTPS redirection in development for testing
if (!builder.Environment.IsDevelopment())
{
    builder.Services.AddHttpsRedirection(options =>
    {
        options.HttpsPort = 5001;
    });
}
// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()      // Allow any origin
               .AllowAnyHeader()     // Allow any headers
               .AllowAnyMethod();    // Allow any HTTP method
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseJwtAuthenticationAndAuthorization();
app.MapControllers();
app.UseCors("AllowAllOrigins");

app.Run();

