using System.Net.Mail;
using System.Text;

namespace Black.Infrastructure.Helpers.MailHelper.MailSystem.Abstract;

public interface IEmailMessage
{
    IEmailMessage BccMailAddresses(List<MailAddress> bccMailAddresses);
    IEmailMessage CcMailAddresses(List<MailAddress> ccMailAddresses);
    IEmailMessage FromMailAddresses(MailAddress fromMailAddress);
    IEmailMessage ToMailAddresses(List<MailAddress> toMailAddresses);
    IEmailMessage WithTheseAttachments(List<Attachment> attachments);
    IEmailBodySetter SetUpBody();
    IMailCredentials WithCredentials();
    IMailer UsingTheInjectedCredentials();
    IEmailMessage Subject(string subject);
    IEmailMessage SubjectEncoding(Encoding encoding);
    IEmailMessage SetPriority(MailPriority mailPriority);
    IEmailMessage ReplyTo(MailAddress replyToEmail);
    MailMessage GetMessage();
}
