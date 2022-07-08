using Black.Infrastructure.Exceptions;
using Black.Infrastructure.Helpers.HttpHelper.Abstract;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Black.Infrastructure.Helpers.HttpHelper.Concrete;

public class HttpHelper : IHttpHelper
{
    #region Members
    private readonly HttpClient _client;
    public HttpRequestHeaders DefaultRequestHeaders => _client.DefaultRequestHeaders;
    public Uri BaseAddress
    {
        get => _client.BaseAddress;
        set => _client.BaseAddress = value;
    }
    public long MaxResponseContentBufferSize
    {
        get => _client.MaxResponseContentBufferSize;
        set => _client.MaxResponseContentBufferSize = value;
    }
    public TimeSpan Timeout
    {
        get => _client.Timeout;
        set => _client.Timeout = value;
    }
    #endregion

    #region Ctor
    /// <summary>
    /// This conctructor creates a httpclient from scratch.
    /// </summary>
    /// <param name="name">Client Name</param>
    /// <param name="clientFactory">Client Factory</param>
    public HttpHelper(string name, IHttpRestClientFactory clientFactory)
    {
        _client = clientFactory.CreateClient(name);
    }

    /// <summary>
    /// This conctructor creates a httpclient from scratch with configuration.
    /// </summary>
    /// <param name="name">Client Name</param>
    /// <param name="clientFactory">Client Factory</param>
    /// <param name="configureHttpClient">Client Configuration</param>
    public HttpHelper(string name, IHttpRestClientFactory clientFactory, Action<HttpClient> configureHttpClient)
    {
        _client = clientFactory.CreateClient(name, configureHttpClient);
    }
    #endregion

    #region GET
    /// <summary>
    /// Sends a get request to the specified method.
    /// </summary>
    /// <param name="method">Method Name</param>
    /// <returns>TResponse</returns>
    public TResponse Get<TResponse>(string method)
    {
        return GetAsync<TResponse>(method).Result;
    }

    /// <summary>
    /// Sends a get request asynchronously to the specified method.
    /// </summary>
    /// <param name="method">Method Name</param>
    /// <returns>TResponse</returns>
    public async Task<TResponse> GetAsync<TResponse>(string method)
    {
        var httpResponseMessage = await _client.GetAsync(method).ConfigureAwait(false);

        Validate(httpResponseMessage, method);

        var JSONResult = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

        return JsonConvert.DeserializeObject<TResponse>(JSONResult);
    }

    #endregion

    #region POST
    /// <summary>
    /// Sends a post request to the specified method.
    /// </summary>
    /// <param name="method">Method Name</param>
    /// <param name="request">Request(JSON Parameters)</param>
    public void Post<TRequest>(string method, TRequest request)
    {
        SendAsync(_client.PostAsync, method, request).Wait();
    }

    /// <summary>
    /// Sends a asynchronous post request to the specified method.
    /// </summary>
    /// <param name="method">Method Name</param>
    /// <param name="request">Request(JSON Parameters)</param>
    public async Task PostAsync<TRequest>(string method, TRequest request)
    {
        await SendAsync(_client.PostAsync, method, request).ConfigureAwait(false);
    }

    /// <summary>
    /// Sends a post request to the specified method.
    /// </summary>
    /// <param name="method">Method Name</param>
    /// <param name="request">Request(JSON Parameters)</param>
    /// <returns>TResponse</returns>
    public TResponse Post<TResponse, TRequest>(string method, TRequest request)
    {
        return SendAsync<TResponse, TRequest>(_client.PostAsync, method, request).Result;
    }

    /// <summary>
    /// Sends a asynchronous post request to the specified method.
    /// </summary>
    /// <param name="method">Method Name</param>
    /// <param name="request">Request(JSON Parameters)</param>
    /// <returns>TResponse</returns>
    public async Task<TResponse> PostAsync<TResponse, TRequest>(string method, TRequest request)
    {
        return await SendAsync<TResponse, TRequest>(_client.PostAsync, method, request).ConfigureAwait(false);
    }

    #endregion

    #region PUT
    /// <summary>
    /// Sends a asynchronous put request to the specified method.
    /// </summary>
    /// <param name="method">Method Name</param>
    /// <param name="request">Request(JSON Parameters)</param>
    /// <returns>TResponse</returns>
    public async Task<TResponse> PutAsync<TResponse, TRequest>(string method, TRequest request)
    {
        return await SendAsync<TResponse, TRequest>(_client.PutAsync, method, request).ConfigureAwait(false);
    }

    /// <summary>
    /// Sends a put request to the specified method.
    /// </summary>
    /// <param name="method">Method Name</param>
    /// <param name="request">Request(JSON Parameters)</param>
    /// <returns>TResponse</returns>
    public TResponse Put<TResponse, TRequest>(string method, TRequest request)
    {
        return SendAsync<TResponse, TRequest>(_client.PutAsync, method, request).Result;
    }

    /// <summary>
    /// Sends a asynchronous put request to the specified method.
    /// </summary>
    /// <param name="method">Method Name</param>
    /// <param name="request">Request(JSON Parameters)</param>
    public async Task PutAsync<TRequest>(string method, TRequest request)
    {
        await SendAsync(_client.PutAsync, method, request).ConfigureAwait(false);
    }

    /// <summary>
    /// Sends a put request to the specified method.
    /// </summary>
    /// <param name="method">Method Name</param>
    /// <param name="request">Request(JSON Parameters)</param>
    public void Put<TRequest>(string method, TRequest request)
    {
        SendAsync(_client.PutAsync, method, request).Wait();
    }

    #endregion

    #region Send
    /// <summary>
    /// Sends a request to the specified method.
    /// </summary>
    /// <param name="request">Request</param>
    /// <returns>HttpResponseMessage</returns>
    public HttpResponseMessage Send(HttpRequestMessage request)
    {
        return SendAsync(request).Result;
    }

    /// <summary>
    /// Sends a asynchronous request to the specified method.
    /// </summary>
    /// <param name="request">Request</param>
    /// <returns>HttpResponseMessage</returns>
    public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
    {
        return await _client.SendAsync(request).ConfigureAwait(false);
    }
    #endregion

    #region Methods       
    /// <summary>
    /// Creates a request and validate response message.
    /// </summary>
    /// <param name="method">Method Name</param>
    /// <param name="methodAsync">Async Method</param>
    /// <param name="request">Request(JSON Parameters)</param>
    private async Task SendAsync<TRequest>(Func<string, JsonContent, Task<HttpResponseMessage>> methodAsync,
                                           string method, TRequest request)
    {
        var httpResponseMessage = await methodAsync(method, new JsonContent(request)).ConfigureAwait(false);
        Validate(httpResponseMessage, method);
    }

    /// <summary>
    /// Creates a asynchronous request and validate response message.
    /// </summary>
    /// <param name="methodAsync">Method</param>
    /// <param name="request">Request(JSON Parameters)</param>
    /// <returns>TResponse</returns>
    private async Task<TResponse> SendAsync<TResponse, TRequest>(Func<string, JsonContent, 
                  Task<HttpResponseMessage>> methodAsync,string path, TRequest request)
    {
        var response = await methodAsync(path, new JsonContent(request)).ConfigureAwait(false);
        Validate(response, path);
        return await ReadAsync<TResponse>(response).ConfigureAwait(false);
    }

    /// <summary>
    /// Validates a simple response message.
    /// </summary>
    /// <param name="response">Response Message</param>
    /// <param name="path">Path</param>
    private void Validate(HttpResponseMessage response, string path)
    {
        if (!response.IsSuccessStatusCode)
            throw new HttpRestClientException(response);
    }

    /// <summary>
    /// Reads a response content message and deserializes it.
    /// </summary>
    /// <param name="response">Response Message</param>
    /// <param name="path">Path</param>
    /// <returns>TResponse</returns>
    private async Task<TResponse> ReadAsync<TResponse>(HttpResponseMessage response)
    {
        var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        return JsonConvert.DeserializeObject<TResponse>(responseString);
    }

    #endregion
}