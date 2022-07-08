using Black.Infrastructure.Helpers.MailHelper.MailSystem.Abstract;

namespace Black.Infrastructure.Helpers.MailHelper.MailSystem.Concrete;

public class MailCredentialSettings : IMailCredentialSettings
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string PortNumber { get; set; }
    public string HostServer { get; set; }
}

