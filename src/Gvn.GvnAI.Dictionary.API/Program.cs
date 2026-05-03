using Gvn.GvnAI.Dictionary.Application.DependencyInjection;
using Gvn.GvnAI.Dictionary.Infrastructure.DependencyInjection;
using Gvn.GvnFramework.AspNetCore.Extensions;
using Gvn.GvnFramework.BackgroundJobs.DependencyInjection;
using Gvn.GvnFramework.Caching.DependencyInjection;
using Gvn.GvnFramework.Security.DependencyInjection;
using Gvn.GvnFramework.Swagger.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// ── Controllers ───────────────────────────────────────────────────────────────
builder.Services.AddControllers();

// ── Application (MediatR, FluentValidation, Behaviors) ────────────────────────
builder.Services.AddDictionaryApplication();

// ── Infrastructure (DbContext, Repositories, AI Service) ──────────────────────
builder.Services.AddDictionaryInfrastructure(builder.Configuration);

// ── Security (JWT) ────────────────────────────────────────────────────────────
builder.Services.AddGvnSecurity(jwt =>
{
    jwt.Secret = builder.Configuration["Jwt:Secret"]!;
    jwt.Issuer = builder.Configuration["Jwt:Issuer"]!;
    jwt.Audience = builder.Configuration["Jwt:Audience"]!;
    jwt.ExpiryMinutes = int.Parse(builder.Configuration["Jwt:ExpiryMinutes"] ?? "60");
});

// ── Caching (Redis) ───────────────────────────────────────────────────────────
builder.Services.AddGvnCaching(cache =>
{
    cache.UseRedis = bool.Parse(builder.Configuration["Cache:UseRedis"] ?? "false");
    cache.ConnectionString = builder.Configuration["Cache:ConnectionString"];
    cache.DefaultExpiryMinutes = int.Parse(builder.Configuration["Cache:DefaultExpiryMinutes"] ?? "30");
    cache.Prefix = builder.Configuration["Cache:Prefix"] ?? "dict:";
});

// ── Background Jobs (Hangfire) ────────────────────────────────────────────────
builder.Services.AddGvnBackgroundJobs(hf =>
{
    hf.UseInMemory = true;
    hf.WorkerCount = 3;
    hf.EnableDashboard = true;
});

// ── Scalar / OpenAPI ──────────────────────────────────────────────────────────
builder.Services.AddGvnSwagger("Gvn.GvnAI.Dictionary API", "v1");

// ── CORS ──────────────────────────────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        var origins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
            ?? new[] { "http://localhost:4200" };
        policy.WithOrigins(origins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// ── Middleware ─────────────────────────────────────────────────────────────────
app.UseGvnCorrelationId();
app.UseGvnExceptionHandling();

app.UseRouting();
app.UseCors("AllowAngular");

app.UseAuthentication();
app.UseAuthorization();

// ── Hangfire Dashboard → /hangfire ────────────────────────────────────────────
app.UseGvnHangfireDashboard("/hangfire");

// ── Scalar UI → /scalar/v1 ───────────────────────────────────────────────────
app.UseGvnSwagger();

app.MapControllers();

app.Run();
