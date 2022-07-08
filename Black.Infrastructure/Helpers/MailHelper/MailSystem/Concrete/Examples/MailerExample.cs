using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace Black.Infrastructure.Helpers.MailHelper.MailSystem.Concrete.Examples;

public class MailerExample
{
    readonly string hostName = string.Empty;
    readonly string portNumber = string.Empty;
    readonly string userName = string.Empty;
    readonly string password = string.Empty;
    readonly string toEmail = string.Empty;
    readonly string ccEmail = string.Empty;
    readonly string bccEmail = string.Empty;

    /// <summary>
    /// Example for sending mail with string body.
    /// </summary>
    /// <returns></returns>
    public void SendMailWithStringBodyNoAttachment()
    {
        var emailIsSent = new Mailer()
                .SetUpMessage()
                    .Subject("Fluent Email Subject : No Attachments")
                    .FromMailAddresses(new MailAddress(userName, "Fluent Email - No Attachments"))
                    .ToMailAddresses(new List<MailAddress> { new MailAddress(toEmail) })
                    .SetUpBody()
                        .SetBodyEncoding(Encoding.UTF8)
                        .SetBodyTransferEncoding(TransferEncoding.Unknown)
                        .Body()
                            .UsingString("This is me Testing")
                            .SetBodyIsHtmlFlag()
               .SetPriority(MailPriority.Normal)
               .WithCredentials()
                    .UsingHostServer(hostName)
                    .OnPortNumber(portNumber)
                    .WithUserName(userName)
                    .WithPassword(password)
                .Send();
    }
    /// <summary>
    /// Example for sending mail with string body and attachment(file).
    /// </summary>
    /// <returns></returns>
    public void SendMailWithStringBodyAndAttachment()
    {
        var emailIsSent = new Mailer()
                           .SetUpMessage()
                                .Subject("Fluent Email Subject : With Attachments")
                                .FromMailAddresses(new MailAddress(userName, "Fluent Email - With Attachments"))
                                .ToMailAddresses(new List<MailAddress> { new MailAddress(toEmail) })
                                .WithTheseAttachments(new List<Attachment> { new Attachment($"{Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SamplePDF.pdf")}") })
                               .SetUpBody()
                                   .SetBodyEncoding(Encoding.UTF8)
                                   .SetBodyTransferEncoding(TransferEncoding.Unknown)
                                   .Body()
                                       .UsingString("This is me Testing")
                                       .SetBodyIsHtmlFlag()
                          .SetPriority(MailPriority.Normal)
                          .WithCredentials()
                               .UsingHostServer(hostName)
                               .OnPortNumber(portNumber)
                               .WithUserName(userName)
                               .WithPassword(password)
                           .Send();
    }
    /// <summary>
    /// Example for sending mail with Bcc and Cc values.
    /// </summary>
    /// <returns></returns>
    public void SendMailWithStringBodyWithBccAndCcEmails()
    {
        var emailIsSent = new Mailer()
                          .SetUpMessage()
                               .Subject("Fluent Email Subject : No Attachments - Bcc and CC Emails")
                               .FromMailAddresses(new MailAddress(userName, "Fluent Email - No Attachments - Bcc and CC Emails"))
                               .ToMailAddresses(new List<MailAddress> { new MailAddress(toEmail) })
                               .CcMailAddresses(new List<MailAddress> { new MailAddress(ccEmail) })
                               .BccMailAddresses(new List<MailAddress> { new MailAddress(bccEmail) })
                               .SetUpBody()
                                  .SetBodyEncoding(Encoding.UTF8)
                                  .SetBodyTransferEncoding(TransferEncoding.Unknown)
                                  .Body()
                                      .UsingString("Email body as string")
                                      .SetBodyIsHtmlFlag()
                         .SetPriority(MailPriority.Normal)
                         .WithCredentials()
                              .UsingHostServer(hostName)
                              .OnPortNumber(portNumber)
                              .WithUserName(userName)
                              .WithPassword(password)
                          .Send();
    }
    /// <summary>
    /// Example for sending e-mail with mail template.
    /// </summary>
    /// <returns></returns>
    public void SendMailUsingEmailTemplateWithNoAttachments()
    {
        var emailIsSent = new Mailer()
                        .SetUpMessage()
                             .Subject("Fluent Email Subject : Template - No Attachments")
                             .FromMailAddresses(new MailAddress(userName, "Fluent Email - Template - No Attachments"))
                             .ToMailAddresses(new List<MailAddress> { new MailAddress(toEmail) })
                             .SetUpBody()
                                .SetBodyEncoding(Encoding.UTF8)
                                .SetBodyTransferEncoding(TransferEncoding.Unknown)
                                .Body()
                                     .UsingEmailTemplate($"{Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestHtmlPage.html")}")
                                     .UsingTemplateDictionary(new Dictionary<string, string> { { "{{subject}}", "Testing Message" }, { "{{body}}", "<section><h2>This is</h2><p>Welcome to our world</p></section>" } })
                                     .CompileTemplate()
                                     .SetBodyIsHtmlFlag()
                       .SetPriority(MailPriority.Normal)
                       .WithCredentials()
                            .UsingHostServer(hostName)
                            .OnPortNumber(portNumber)
                            .WithUserName(userName)
                            .WithPassword(password)
                        .Send();
    }
    /// <summary>
    /// Example for sending e-mail with mail template with attachment(file).
    /// </summary>
    /// <returns></returns>
    public void SendMailUsingEmailTemplateWithAttachments()
    {
        var emailIsSent = new Mailer()
                        .SetUpMessage()
                             .Subject("Fluent Email Subject : Template - With Attachments")
                             .FromMailAddresses(new MailAddress(userName, "Fluent Email - Template - With Attachments"))
                             .ToMailAddresses(new List<MailAddress> { new MailAddress(toEmail) })
                             .WithTheseAttachments(new List<Attachment> { new Attachment($"{Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SamplePDF.pdf")}") })
                             .SetUpBody()
                                .SetBodyEncoding(Encoding.UTF8)
                                .SetBodyTransferEncoding(TransferEncoding.Unknown)
                                .Body()
                                     .UsingEmailTemplate($"{Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestHtmlPage.html")}")
                                     .UsingTemplateDictionary(new Dictionary<string, string> { { "{{subject}}", "Testing Message" }, { "{{body}}", "<section><h2>This is</h2><p>Welcome to our world</p></section>" } })
                                     .CompileTemplate()
                                     .SetBodyIsHtmlFlag()
                       .SetPriority(MailPriority.Normal)
                       .WithCredentials()
                            .UsingHostServer(hostName)
                            .OnPortNumber(portNumber)
                            .WithUserName(userName)
                            .WithPassword(password)
                        .Send();
    }
    /// <summary>
    /// Example for sending e-mail with mail template with attachment(file).
    /// </summary>
    /// <returns></returns>
    public void SendMailWithReplyToSubjectEncodingBodyEncoding()
    {
        var emailIsSent = new Mailer()
                .SetUpMessage()
                    .Subject("Fluent Email Subject : No Attachments - With ReplyTo")
                    .FromMailAddresses(new MailAddress(userName, "Fluent Email - No Attachments - With ReplyTo"))
                    .ToMailAddresses(new List<MailAddress> { new MailAddress(toEmail) })
                    .SubjectEncoding(Encoding.UTF8)
                    .ReplyTo(new MailAddress("info@creativemode.co.za"))
                    .SetUpBody()
                        .SetBodyEncoding(Encoding.UTF8)
                        .SetBodyTransferEncoding(TransferEncoding.Unknown)
                        .Body()
                            .UsingString("This is me Testing")
                            .SetBodyIsHtmlFlag()
               .SetPriority(MailPriority.Normal)
               .WithCredentials()
                    .UsingHostServer(hostName)
                    .OnPortNumber(portNumber)
                    .WithUserName(userName)
                    .WithPassword(password)
                .Send();
    }
    /// <summary>
    /// Example for sending mail with injected credentials.
    /// </summary>
    /// <returns></returns>
    public void SendMailUsingInjectedCredentials()
    {
        var emailIsSent = new Mailer(new MailCredentials { PortNumber = portNumber, HostServer = hostName, Password = password, UserName = userName })
                .SetUpMessage()
                    .Subject("Fluent Email Subject : Injected  Credentials")
                    .FromMailAddresses(new MailAddress(userName, "Fluent Email - Injected  Credentials"))
                    .ToMailAddresses(new List<MailAddress> { new MailAddress(toEmail) })
                    .SubjectEncoding(Encoding.UTF8)
                    .ReplyTo(new MailAddress("info@creativemode.co.za"))
                    .SetUpBody()
                        .SetBodyEncoding(Encoding.UTF8)
                        .SetBodyTransferEncoding(TransferEncoding.Unknown)
                        .Body()
                            .UsingString("This is me Testing")
                            .SetBodyIsHtmlFlag()
               .SetPriority(MailPriority.Normal)
               .UsingTheInjectedCredentials()
               .Send();
    }

}
