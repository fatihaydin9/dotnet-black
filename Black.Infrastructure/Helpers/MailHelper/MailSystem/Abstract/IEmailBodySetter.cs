using System.Net.Mime;
using System.Text;

namespace Black.Infrastructure.Helpers.MailHelper.MailSystem.Abstract;

public interface IEmailBodySetter
{
    IEmailTemplate Body();
    IEmailBodySetter SetBodyTransferEncoding(TransferEncoding transferEncoding);
    IEmailBodySetter SetBodyEncoding(Encoding encoding);
    IEmailMessage SetBodyIsHtmlFlag(bool emailBodyIsHtml = true);
}
