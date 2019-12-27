using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using FoodStandardsAgency.Rating.Contracts;
using ServiceClient;
using ServiceClient.Contracts;
using ServiceClient.Models;

namespace FoodStandardsAgency.Rating
{
    /// <summary>
    /// Food Hygiene Rating calculator.
    /// 
    /// Given a authority ID, finds all establishments for that authority
    /// Calculate how many establishments have that rating and what overall 
    /// percentage of establishments have that rating.
    /// 
    /// </summary>
    public class RatingCalculator : IRatingCalculator
    {
        private IFoodStandardAgencyServiceClient _serviceClient;
        private ILogger _logger;
        private IRatingFactory _ratingFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:FoodStandardsAgency.Rating.RatingCalculator"/> class.
        /// </summary>
        /// <param name="serviceClient">Service client used to connect to service</param>
        /// <param name="logger">Logger</param>
        /// <param name="ratingFactory">Rating factory used to generate the required rating system</param>
        public RatingCalculator(IFoodStandardAgencyServiceClient serviceClient, ILogger<RatingCalculator> logger, IRatingFactory ratingFactory)
        {
            if (serviceClient == null)
            {
                throw new ArgumentNullException(nameof(serviceClient));
            }
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }
            if (ratingFactory == null)
            {
                throw new ArgumentNullException(nameof(ratingFactory));
            }

            _serviceClient = serviceClient;
            _logger = logger;
            _ratingFactory = ratingFactory;
        }

        /// <summary>
        /// Calculate the ratings of establishments for a given authority Id.
        /// </summary>
        /// <returns>List of ratings for establishments within the authority.</returns>
        /// <param name="authorityId">Authority identifier.</param>
        public List<AuthorityRating> Calculate(int authorityId)
        {
            Authority authority;
            List<Establishment> establishments;

            try
            {
                // get the authority details and a list of establishments for that authority.
                authority = this.GetAuthority(authorityId);
                establishments = this.GetEstablishments(authorityId);
            }
            catch (ServiceException ex)
            {
                throw new RatingCalculatorException("Service exception thrown in Rating Calculator ", ex);
            }

            // get the rating system for the given region
            List<AuthorityRating> ratings = _ratingFactory.GetRatings(authority.RegionName);

            // interate around the list of establishments
            // updating the rating count for whatever rating the establishments has. 
            int processedEstablishments = 0;
            foreach (var es in establishments)
            {
                var rating = ratings.FirstOrDefault(o => o.RatingKey == es.RatingValue);
                if (rating != null)
                {
                    rating.RatingCount++;
                    processedEstablishments++;
                }
            }

            // now we have a complete count establishments which fit into the rating system
            // we work out what the percentage of establishments have each rating.
            foreach (var item in ratings)
            {
                item.Percentage = ((item.RatingCount / processedEstablishments) * 100);
            }

            return ratings;
        }

        /// <summary>
        /// Call the asynchronous method to get the Authority  
        /// </summary>
        /// <returns>Instance of an authority.</returns>
        /// <param name="authorityId">Authority id</param>
        private Authority GetAuthority(int authorityId)
        {
            return this.GetAuthorityAsync(authorityId).Result;
        }

        /// <summary>
        /// Call the asynchronous to get the establishments for a given authority Id
        /// </summary>
        /// <returns>List of establishments </returns>
        /// <param name="authorityId">Authority Id.</param>
        private List<Establishment> GetEstablishments(int authorityId)
        {
            _logger.LogInformation(String.Format("Retrieving Establishments Directly From FSA Service For Authority Id {0}", authorityId));
            List<Establishment> results = this.GetEstablishmentAsync(authorityId).Result.Items;
            _logger.LogInformation(String.Format("Returned {0} Establishments From GetEstablishments for Authority Id {1} ", results.Count(), authorityId));
            return results;
        }

        /// <summary>
        /// Calls the asynchronous service method to get the authority
        /// </summary>
        /// <returns>Task with Authority result</returns>
        /// <param name="authorityId">Authority Id.</param>
        private async Task<Authority> GetAuthorityAsync(int authorityId)
        {
            return await _serviceClient.GetAuthority(authorityId);
        }

        /// <summary>
        /// Calls the asynchronous service method to get a List of establishment
        /// </summary>
        /// <returns>Task with Establishments result</returns>
        /// <param name="authorityId">Authority id.</param>
        private async Task<Establishments> GetEstablishmentAsync(int authorityId)
        {
            return await _serviceClient.GetEstablishments(authorityId);
        }
    }
}
