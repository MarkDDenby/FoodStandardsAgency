using System;
using System.Collections.Generic;
using System.Net.Http;
using ServiceClient.Contracts;

namespace ServiceClient
{
    /// <summary>
    /// Base Service client used to connect to a service
    /// </summary>
    public abstract class ServiceClientBase
    {
        private readonly HttpClient httpClient;                           
        public Dictionary<String, String> Headers { protected set; get; } // collection of headers in the format header key, header value 

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ServiceClient.ServiceClientBase"/> class.
        /// </summary>
        /// <param name="httpClientFactory">Http client factory used to connect to the service</param>
        public ServiceClientBase(IHttpClientFactory httpClientFactory)
        {
            if(httpClientFactory == null)
            {
                throw new ArgumentNullException(nameof(httpClientFactory));
            }

            this.httpClient = httpClientFactory.Client;
            this.Headers = new Dictionary<string, string>();
        }


        /// <summary>
        /// Returns an instance of HttpResponseMessage
        /// </summary>
        /// <returns>The response to the http request</returns>
        /// <param name="serviceEndPointUri">Service end point uri</param>
        protected HttpResponseMessage GetResponseMessage(string serviceEndPointUri)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = this.httpClient.SendAsync(this.RequestMessage(serviceEndPointUri)).Result;
                response.EnsureSuccessStatusCode(); // Throw an exception if response did not return a success code.
            }
            catch (HttpRequestException ex)
            {
                ServiceException serviceException = new ServiceException("Could not connect to service", ex);
                throw serviceException;
            }
            return response;
        }

        /// <summary>
        /// Returns an instance of HttpRequestMessage configure for the supplied end point.
        /// Configures message with the previously configured headers
        /// </summary>
        /// <returns>HttpRequestMessage</returns>
        /// <param name="serviceEndPointUri">URI.</param>
        protected HttpRequestMessage RequestMessage(string serviceEndPointUri)
        {
            // configure request message
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(serviceEndPointUri),
                Method = HttpMethod.Get,
            };

            // apply header values to request
            foreach (var header in Headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }
            return request;
        }
    }
}
