namespace Black.Infrastructure.Helpers.MailHelper.MailSystem.Abstract;

public interface IMailer
{
    IEmailMessage SetUpMessage();
    IEmailMessage Message { get; }
    IMailCredentials MailCredentials { get; }
    bool Send();
    Task<bool> SendAsync();
}
