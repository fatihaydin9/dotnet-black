using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace Black.Infrastructure.Attributes;

public sealed class Authorize : ActionFilterAttribute
{
    private string[] Keys
    {
        get;
        set;
    }

    public Authorize()
    {
    }

    public Authorize(string keys)
    {
        Keys = keys.Split(",");
    }

    public Authorize(params string[] keys)
    {
        Keys = keys;
    }

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        string controller = ((ControllerBase)filterContext.Controller).ControllerContext.ActionDescriptor.ControllerName;
        string action = ((ControllerBase)filterContext.Controller).ControllerContext.ActionDescriptor.ActionName;
        IConfiguration? service = filterContext.HttpContext.RequestServices.GetService(typeof(IConfiguration)) as IConfiguration;
        IConfigurationSection section = service.GetSection("ApplicationName");
        string value = section.Value;
        string key = service["ConnectToken:SecurityKey"].ToString();
        string auth = filterContext.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        string pageInfo = filterContext.HttpContext.Request.Headers["PageInfo"].ToString();
    }
}
