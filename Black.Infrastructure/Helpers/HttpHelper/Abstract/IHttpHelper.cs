﻿using System.Net.Http.Headers;

namespace Black.Infrastructure.Helpers.HttpHelper.Abstract;

public interface IHttpHelper
{
    HttpRequestHeaders DefaultRequestHeaders { get; }
    Uri BaseAddress { get; set; }
    long MaxResponseContentBufferSize { get; set; }
    TimeSpan Timeout { get; set; }

    TResponse Get<TResponse>(string method);
    Task<TResponse> GetAsync<TResponse>(string method);
    void Post<TRequest>(string method, TRequest request);
    TResponse Post<TResponse, TRequest>(string method, TRequest request);
    Task PostAsync<TRequest>(string method, TRequest request);
    Task<TResponse> PostAsync<TResponse, TRequest>(string method, TRequest request);
    void Put<TRequest>(string method, TRequest request);
    TResponse Put<TResponse, TRequest>(string method, TRequest request);
    Task PutAsync<TRequest>(string method, TRequest request);
    Task<TResponse> PutAsync<TResponse, TRequest>(string method, TRequest request);
    HttpResponseMessage Send(HttpRequestMessage request);
    Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
}