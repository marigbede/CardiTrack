using CardiTrack.API.Extensions;
using CardiTrack.API.Middleware;
using CardiTrack.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Serilog;
using AspNetCoreRateLimit;
using FluentValidation.AspNetCore;

// Configure Serilog
var builder = WebApplication.CreateBuilder(args);
builder.AddSerilogLogging();

try
{
    Log.Information("Starting CardiTrack API");

    // 1. DATABASE
    builder.Services.AddDbContext<CardiTrackDbContext>(options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection"),
            b => b.MigrationsAssembly("CardiTrack.Infrastructure")));

    // 2. AUTHENTICATION & AUTHORIZATION - Auth0 JWT
    builder.Services.AddAuth0Authentication(builder.Configuration);
    builder.Services.AddAuth0Authorization();

    // 3. CONTROLLERS & VALIDATION
    builder.Services.AddControllers();
    builder.Services.AddFluentValidationAutoValidation();
    builder.Services.AddFluentValidationClientsideAdapters();

    // 4. API VERSIONING
    builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
    });

    // 5. SWAGGER/OPENAPI - With JWT support
    builder.Services.AddSwaggerWithJwtSupport(builder.Configuration);

    // 6. APPLICATION SERVICES
    builder.Services.AddApplicationServices();
    builder.Services.AddInfrastructureServices(builder.Configuration);

    // 7. USER CONTEXT
    builder.Services.AddUserContextServices();

    // 8. CACHING (Redis + In-Memory)
    builder.Services.AddCachingServices(builder.Configuration);

    // 9. RATE LIMITING
    builder.Services.AddRateLimiting(builder.Configuration);

    // 10. CORS
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowSpecificOrigins", policy =>
        {
            policy.WithOrigins(
                    builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
                    ?? Array.Empty<string>())
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
    });

    // 11. HEALTH CHECKS
    builder.Services.AddHealthChecks();
    // .AddDbContextCheck<CardiTrackDbContext>("database")
    // .AddRedis(builder.Configuration.GetConnectionString("Redis") ?? "localhost:6379", "redis");

    // 12. AUTOMAPPER
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    var app = builder.Build();

    // AUTO-MIGRATE DATABASE ON STARTUP
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<CardiTrackDbContext>();
        try
        {
            Log.Information("Checking database migrations...");
            var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();

            if (pendingMigrations.Any())
            {
                Log.Information("Applying {Count} pending migrations", pendingMigrations.Count());
                await dbContext.Database.MigrateAsync();
                Log.Information("Database migrations applied successfully");
            }
            else
            {
                Log.Information("Database is up to date");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while migrating the database");
            throw;
        }
    }

    // MIDDLEWARE PIPELINE
    app.UseHttpsRedirection();
    app.UseSerilogRequestLogging();
    app.UseMiddleware<ExceptionHandlingMiddleware>();
    app.UseIpRateLimiting();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "CardiTrack API V1");
            c.RoutePrefix = string.Empty;  // Serve Swagger at root
        });
    }

    app.UseCors("AllowSpecificOrigins");
    app.UseAuthentication();
    app.UseMiddleware<UserContextMiddleware>();
    app.UseAuthorization();
    app.MapControllers();
    app.MapHealthChecks("/health");

    Log.Information("CardiTrack API started successfully");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application failed to start");
    throw;
}
finally
{
    Log.CloseAndFlush();
}
