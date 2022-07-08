namespace Black.Infrastructure.Helpers.MailHelper.MailSystem.Abstract;

public interface IEmailTemplate
{
    IEmailBodySetter CompileTemplate();
    IEmailTemplate UsingEmailTemplate(string templateLocation);
    IEmailTemplate UsingEmailTemplate(string templateLocation, Dictionary<string, string> templateValues);
    IEmailBodySetter UsingString(string emailBody);
    IEmailTemplate UsingTemplateDictionary(Dictionary<string, string> templateValues);
}
