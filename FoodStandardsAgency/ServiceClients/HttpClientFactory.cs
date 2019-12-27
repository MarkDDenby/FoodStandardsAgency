using System.Net.Http;
using ServiceClient.Contracts;

namespace ServiceClient
{
    /// <summary>
    /// Factory that returns an instance of HttpClient
    /// </summary>
    public class HttpClientFactory : IHttpClientFactory
    {
        public HttpClient Client { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ServiceClient.HttpClientFactory"/> class.
        /// </summary>
        public HttpClientFactory()
        {
            Client = new HttpClient();
        }
    }
}
