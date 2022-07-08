using Prometheus;
using DotNetCore.IoC;
using DotNetCore.Security;
using DotNetCore.AspNetCore;
using HealthChecks.UI.Client;
using Microsoft.OpenApi.Models;
using Black.Service.UserService;
using Black.Database.Repositories.Abstract;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Black.Infrastructure.UnitOfWorkBase.Concrete;
using Black.Infrastructure.UnitOfWorkBase.Abstract;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;


#region BUILDER
var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
services.AddClassesMatchingInterfaces(
    typeof(IUserService).Assembly,
    typeof(IUserRepository).Assembly
);
#endregion


#region SERVICES
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Black.UserApi", Version = "v1" });
});
services.AddHashService();
services.AddHealthChecks();
services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddLogging();
services.AddMemoryCache();
services.AddDbContext<RelationalDbContext>();
services.AddTransient<IUnitOfWork, UnitOfWork>();
services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
services.AddAuthenticationJwtBearer(new JwtSettings(Guid.NewGuid().ToString(), TimeSpan.FromHours(12)));
#endregion 


#region APPMETRICS
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


#region APPSETTINGS
var app = builder.Build();
app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "User API V1");
    });
}

app.UseHttpMetrics();

app.MapMetrics();

app.Run();
#endregion
