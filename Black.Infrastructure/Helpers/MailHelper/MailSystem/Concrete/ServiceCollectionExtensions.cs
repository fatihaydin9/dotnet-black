using Black.Infrastructure.Helpers.MailHelper.MailSystem.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace Black.Infrastructure.Helpers.MailHelper.MailSystem.Concrete;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds a Fluent Mail System for user-notification system.
    /// </summary>
    /// <param name="services">Fluent Mail Service</param>
    /// <returns>IServiceCollection</returns>
    public static IServiceCollection AddFluentEmailer(this IServiceCollection services)
    {
        services.AddScoped<IMailer, Mailer>();
        return services;
    }
}
