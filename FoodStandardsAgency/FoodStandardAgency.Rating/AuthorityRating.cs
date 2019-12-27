namespace FoodStandardsAgency.Rating
{
    /// <summary>
    /// Holds the rating details
    /// </summary>
    public class AuthorityRating
    {
        /// <summary>
        /// Gets or sets the rating key.
        /// </summary>
        /// <value>The rating key for the rating system, rating key 1,2,3,4,5 for example</value>
        public string RatingKey { get; set; }

        /// <summary>
        /// Gets or sets the rating count
        /// </summary>
        /// <value>Count of how many establishments have the rating held in RatingKey.</value>
        public float RatingCount { get; set; }

        /// <summary>
        /// Gets or sets the rating image path.
        /// </summary>
        /// <value>The path for the image for this rating held in RatingKey.</value>
        public string RatingImagePath { get; set; }

        /// <summary>
        /// Gets or sets the percentage.
        /// </summary>
        /// <value>Holds an overall percentage of how many establishments have this rating</value>
        public float Percentage { get; set; }
    }
}
