namespace Black.Infrastructure.Helpers.MailHelper.MailSystem.Abstract;

public interface IMailCredentialSettings
{
    string UserName { get; set; }
    string Password { get; set; }
    string PortNumber { get; set; }
    string HostServer { get; set; }
}
