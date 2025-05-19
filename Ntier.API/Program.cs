using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Ntier.API.Filter;
using Ntier.API.Middlewares;
using Ntier.Business;
using Ntier.DataAccess;
using Serilog;

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
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(null));
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Ntier API", Version = "v1" });

    // Force camelCase in Swagger models
    options.DescribeAllParametersInCamelCase();

    // ✅ Display enums as string in Swagger
    options.SchemaFilter<EnumSchemaFilter>();

    // Add JWT Bearer Authorization to Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter 'Bearer {your_token_here}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });


    // 2️⃣ Apply authentication **only** for protected routes
    options.OperationFilter<AuthorizeCheckOperationFilter>();
});

// Create Logger instance
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration) // Load from appsettings.json
    .Enrich.FromLogContext() // Adds context data to logs
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day) // Logs to file
    .CreateLogger();

// Add Serilog to DI
builder.Services.AddSingleton(Log.Logger);
builder.Host.UseSerilog(); // Set serilog as the main logger


// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("*")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Configure Rate Limiting
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 100, // Max requests allowed
                Window = TimeSpan.FromMinutes(1), // Time window
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 10 // Requests to queue before rejecting
            }
        )
    );

    options.OnRejected = async (context, cancellationToken) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        await context.HttpContext.Response.WriteAsync("Too many requests. Please try again later.", cancellationToken);
    };
});

var app = builder.Build();

app.UseSerilogRequestLogging();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DataContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<DataContext>>();
    DbSeed.SeedFood(db, logger);
}


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

// Enable CORS
app.UseCors("AllowSpecificOrigins");

// Enable Rate Limiting
app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();