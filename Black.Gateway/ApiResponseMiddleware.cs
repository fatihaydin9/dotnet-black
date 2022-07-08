using Black.Infrastructure.Extensions;
using Black.Infrastructure.Objects;
using Newtonsoft.Json;
using System.Net;

namespace Black.Gateway;

public class ApiResponseMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
    private readonly IWebHostEnvironment _env;

    public ApiResponseMiddleware(RequestDelegate next, ILogger logger, IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task Invoke(HttpContext context)
    {
        if (IsSwagger(context))
        {
            await this._next(context);
        }
        else
        {
            var originalBodyStream = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                try
                {
                    await _next.Invoke(context);

                    if (context.Response.StatusCode != (int)HttpStatusCode.InternalServerError && context.Response.StatusCode != (int)HttpStatusCode.BadRequest)
                    {
                        var body = await FormatResponse(context.Response);
                        string bodyText = string.Empty;
                        if (!body.ToString().IsValidJson())
                            bodyText = JsonConvert.SerializeObject(body);
                        else
                            bodyText = body.ToString();
                        dynamic bodyContent = JsonConvert.DeserializeObject<dynamic>(bodyText);
                        await HandleSuccessRequestAsync(context, bodyContent, context.Response.StatusCode);
                    }
                }
                catch (Exception exception)
                {
                    _logger.LogInformation("Logged on {PlaceHolderName:MMMM dd, yyyy}", DateTimeOffset.UtcNow);
                    _logger.LogError(_env.EnvironmentName, exception, exception.Message, exception.InnerException);
                }
                finally
                {
                    responseBody.Seek(0, SeekOrigin.Begin);
                    await responseBody.CopyToAsync(originalBodyStream);
                }
            }
        }
    }

    private static Task HandleSuccessRequestAsync(HttpContext context, object bodyContent, int code)
    {
        context.Response.ContentType = "application/json";
        string jsonString;
        ApiResponse apiResponse = null;

        apiResponse = new ApiResponse(code, true, null, bodyContent);
        jsonString = apiResponse.ToString();

        return context.Response.WriteAsync(jsonString);
    }

    private async Task<string> FormatResponse(HttpResponse response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);
        var plainBodyText = await new StreamReader(response.Body).ReadToEndAsync();
        response.Body.Seek(0, SeekOrigin.Begin);

        return plainBodyText;
    }

    private bool IsSwagger(HttpContext context)
    {
        return context.Request.Path.StartsWithSegments("/swagger");
    }

}
