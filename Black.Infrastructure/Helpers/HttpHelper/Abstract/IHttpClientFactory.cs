namespace Black.Infrastructure.Helpers.HttpHelper.Abstract;

public interface IHttpRestClientFactory
{
    HttpClient CreateClient(string name, Action<HttpClient> configureHttpClient = null);
}