using System.Net.Http;

namespace ServiceClient.Contracts
{
    /// <summary>
    /// represents a factory that returns an instance of HttpClient
    /// </summary>
    public interface IHttpClientFactory
    {
        HttpClient Client { get; }
    }
}