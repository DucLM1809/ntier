using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Ntier.API.Middlewares;
using Ntier.Business;
using Ntier.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Load Configuration
var config = builder.Configuration;
var jwtSettings = config.GetSection("JwtSettings");

// Configure Database (PostgreSQL)
builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(config.GetConnectionString("DefaultConnection"))
);

// Configure JWT
var key = Encoding.ASCII.GetBytes(config["JwtSettings:Secret"]);
builder
    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = config["JwtSettings:Issuer"],
            ValidAudience = config["JwtSettings:Audience"]
        };
    });

builder.Services.AddDataAccess(config);
builder.Services.AddBusiness();

// Configure lowercase URLs globally
builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true; // ✅ Enforces lowercase routes
    options.LowercaseQueryStrings = true; // (Optional) Makes query strings lowercase
});

builder
    .Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Ntier API", Version = "v1" });

    // Force camelCase in Swagger models
    options.DescribeAllParametersInCamelCase();
});
;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Register Global Middleware
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<JwtMiddleware>(jwtSettings["Secret"]);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();