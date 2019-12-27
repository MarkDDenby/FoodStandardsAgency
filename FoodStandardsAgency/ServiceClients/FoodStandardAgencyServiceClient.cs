using System;
using System.Net.Http;
using System.Threading.Tasks;
using ServiceClient.Contracts;
using ServiceClient.Models;

namespace ServiceClient
{
    /// <summary>
    /// Service client that connects to the Food Standard Agency service.
    /// </summary>
    public class FoodStandardAgencyServiceClient : ServiceClientBase, IFoodStandardAgencyServiceClient
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:ServiceClient.FoodStandardAgencyServiceClient"/> class.
        /// </summary>
        /// <param name="httpClientFactory">Http client factory.</param>
        public FoodStandardAgencyServiceClient(IHttpClientFactory httpClientFactory) 
            : base(httpClientFactory)
        {
            this.Headers.Add("x-api-version", "2"); // add the required header information for the FSA service
        }

        /// <summary>
        /// Gets all authorities.
        /// </summary>
        /// <returns>Task with Authorities result</returns>
        public async Task<Authorities> GetAllAuthorities()
        {
            string url = "http://api.ratings.food.gov.uk/authorities";

            HttpResponseMessage response = this.GetResponseMessage(url);
            return await response.Content.ReadAsJsonAsync<Authorities>();
        }

        /// <summary>
        /// Returns all establishments for a given authority id
        /// </summary>
        /// <returns>Task with Establishments results</returns>
        /// <param name="authorityId">Authority Id for the establishments to be returned</param>
        public async Task<Establishments> GetEstablishments(int authorityId)
        {
            string url = String.Format("http://api.ratings.food.gov.uk/Establishments?localAuthorityId={0}&pageSize=0", authorityId.ToString());

            HttpResponseMessage response = this.GetResponseMessage(url);
            return await response.Content.ReadAsJsonAsync<Establishments>();
        }

        /// <summary>
        /// Return an authority for a given authority id
        /// </summary>
        /// <returns>Task with an authority result</returns>
        /// <param name="authorityId">Authority identifier.</param>
        public async Task<Authority> GetAuthority(int authorityId)
        {
            string url = String.Format("http://api.ratings.food.gov.uk/authorities/{0}", authorityId.ToString());

            HttpResponseMessage response = this.GetResponseMessage(url);
            return await response.Content.ReadAsJsonAsync<Authority>();
        }
    }
}
