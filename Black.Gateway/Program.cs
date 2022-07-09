using Polly;
using Serilog;
using Black.Common;
using DotNetCore.IoC;
using DotNetCore.Security;
using DotNetCore.AspNetCore;
using Polly.Extensions.Http;
using HealthChecks.UI.Client;
using Black.Service.AuthService;
using Black.Service.FileService;
using Black.Service.UserService;
using MMLib.Ocelot.Provider.AppConfiguration;
using MMLib.SwaggerForOcelot.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Black.Infrastructure.UnitOfWorkBase.Concrete;
using Black.Database.Repositories.Abstract;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Black.Infrastructure.UnitOfWorkBase.Abstract;
using Prometheus;
using Microsoft.AspNetCore.Server.Kestrel.Core;


#region GW_BUILDER
// create
var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;
// logging
builder.Host.UseSerilog();
builder.Logging.AddProvider(new FileLoggerProvider());
builder.Host.ConfigureAppConfiguration(config =>
{
    config.AddOcelotWithSwaggerSupport(options =>
    {
        options.Folder = "OcelotConfiguration";
    });
});

// match interfaces
services.AddClassesMatchingInterfaces(
    typeof(IUserService).Assembly,
    typeof(IUserRepository).Assembly
);
#endregion


#region GW_SERVICES
services.AddHashService();
services.AddHealthChecks();
services.AddDbContext<RelationalDbContext>();
services.AddTransient<IUnitOfWork, UnitOfWork>();
services.AddTransient<LoggingDelegatingHandler>();
services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddOcelot(configuration).AddAppConfiguration();
services.AddSwaggerForOcelot(configuration);

services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
services.AddAuthenticationJwtBearer(new JwtSettings(Guid.NewGuid().ToString(), TimeSpan.FromHours(12)));

services.AddHealthChecks()
              .AddUrlGroup(new Uri($"{configuration["Services:auth:DownstreamPath"]}/swagger/index.html"), "Catalog.API", HealthStatus.Degraded)
              .AddUrlGroup(new Uri($"{configuration["Services:file:DownstreamPath"]}/swagger/index.html"), "Basket.API", HealthStatus.Degraded)
              .AddUrlGroup(new Uri($"{configuration["Services:user:DownstreamPath"]}/swagger/index.html"), "Ordering.API", HealthStatus.Degraded);
#endregion


#region GW_HTTP_CLIENTS
services.AddHttpClient<IAuthService, AuthService>(c =>
    c.BaseAddress = new Uri(configuration["Services:auth:DownstreamPath"]))
    .AddHttpMessageHandler<LoggingDelegatingHandler>()
    .AddPolicyHandler(GetRetryPolicy())
    .AddPolicyHandler(GetCircuitBreakerPolicy());

services.AddHttpClient<IFileService, FileService>(c =>
    c.BaseAddress = new Uri(configuration["Services:file:DownstreamPath"]))
    .AddHttpMessageHandler<LoggingDelegatingHandler>()
    .AddPolicyHandler(GetRetryPolicy())
    .AddPolicyHandler(GetCircuitBreakerPolicy());

services.AddHttpClient<IUserService, UserService>(c =>
    c.BaseAddress = new Uri(builder.Configuration["Services:user:DownstreamPath"]))
    .AddHttpMessageHandler<LoggingDelegatingHandler>()
    .AddPolicyHandler(GetRetryPolicy())
    .AddPolicyHandler(GetCircuitBreakerPolicy());
#endregion


#region GW_METRICS
services.Configure<KestrelServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});

services.Configure<IISServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});

services.AddMetrics();
services.AddMetricsTrackingMiddleware();
#endregion


#region GW_APP_SETTINGS
var app = builder.Build();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
});

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwaggerForOcelotUI(opt =>
    {
        opt.PathToSwaggerGenerator = "/swagger/docs";

    });
}

app.UseOcelot().Wait();
app.MapControllers();
app.UseHttpMetrics();
app.MapMetrics();
app.Run();
#endregion


#region GW_EXTERNAL
static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    // In this case will wait for
    //  2 ^ 1 = 2 seconds then
    //  2 ^ 2 = 4 seconds then
    //  2 ^ 3 = 8 seconds then
    //  2 ^ 4 = 16 seconds then
    //  2 ^ 5 = 32 seconds

    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .WaitAndRetryAsync(
            retryCount: 5,
            sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
            onRetry: (exception, retryCount, context) =>
            {
                Log.Error($"Retry {retryCount} of {context.PolicyKey} at {context.OperationKey}, due to: {exception}.");
            });
}

static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .CircuitBreakerAsync(
            handledEventsAllowedBeforeBreaking: 5,
            durationOfBreak: TimeSpan.FromSeconds(30)
        );
}
#endregion