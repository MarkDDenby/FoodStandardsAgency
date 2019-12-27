using System.Collections.Generic;
using FoodStandardsAgency.Rating.Contracts;

namespace FoodStandardsAgency.Rating
{
    /// <summary>
    /// Rating factory which returns the rating system for a given region.
    /// </summary>
    public class RatingFactory : IRatingFactory
    {
        /// <summary>
        /// Gets the rating system for a given region
        /// </summary>
        /// <returns>List of authority rating for a given region</returns>
        /// <param name="regionName">The Region name.</param>
        public List<AuthorityRating> GetRatings(string regionName)
        {
            List<AuthorityRating> ratings = new List<AuthorityRating>();
            switch (regionName)
            {
                case "Scotland":
                {
                    // Scottish rating system    
                    ratings.Add(new AuthorityRating() { RatingKey = "Pass", RatingImagePath = "~/FsaImages/Scotland/fhis_pass.jpg" });
                    ratings.Add(new AuthorityRating() { RatingKey = "Improvement Required", RatingImagePath = "~/FsaImages/Scotland/fhis_improvement_required.jpg" });
                    break;
                }
                default:
                {
                    // default to the GB rating system    
                    ratings.Add(new AuthorityRating() { RatingKey = "5", RatingImagePath = "~/FsaImages/England/fhrs_5_en-gb.jpg" });
                    ratings.Add(new AuthorityRating() { RatingKey = "4", RatingImagePath = "~/FsaImages/England/fhrs_4_en-gb.jpg" });
                    ratings.Add(new AuthorityRating() { RatingKey = "3", RatingImagePath = "~/FsaImages/England/fhrs_3_en-gb.jpg" });
                    ratings.Add(new AuthorityRating() { RatingKey = "2", RatingImagePath = "~/FsaImages/England/fhrs_2_en-gb.jpg" });
                    ratings.Add(new AuthorityRating() { RatingKey = "1", RatingImagePath = "~/FsaImages/England/fhrs_1_en-gb.jpg" });
                    ratings.Add(new AuthorityRating() { RatingKey = "Exempt", RatingImagePath = "~/FsaImages/England/fhrs_exempt_en-gb.jpg" });
                    break;
                }
            }
            return ratings;
        }
    }
}