using System.Net;

namespace Black.Infrastructure.Exceptions;

public class HttpRestClientException : Exception
{
    public string ReasonPhrase => HttpResponseMessage.ReasonPhrase;
    public HttpStatusCode StatusCode => HttpResponseMessage.StatusCode;
    public HttpResponseMessage HttpResponseMessage { get; }

    /// <summary>
    /// Gets http exception from http response message
    /// </summary>
    public HttpRestClientException(HttpResponseMessage httpResponseMessage) :
        base($"Error invoking URL: {httpResponseMessage.RequestMessage.RequestUri.AbsoluteUri}. {httpResponseMessage.ReasonPhrase}")
    {
        HttpResponseMessage = httpResponseMessage;
    }
}