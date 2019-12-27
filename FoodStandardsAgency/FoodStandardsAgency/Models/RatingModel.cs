using System.ComponentModel.DataAnnotations;

namespace FoodStandardsAgency.Models
{
    /// <summary>
    /// Model to hold the rating.
    /// </summary>
    public class RatingModel
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
        public float Rating { get; set; }

        /// <summary>
        /// Gets or sets the rating image path.
        /// </summary>
        /// <value>The path for the image for this rating held in RatingKey.</value>
        public string RatingImagePath { get; set; }

        /// <summary>
        /// Gets or sets the percentage.
        /// </summary>
        /// <value>Holds an overall percentage of how many establishments have this rating</value>
        [DisplayFormat(DataFormatString = "{0:N2}%")]
        public float Percentage { get; set; }
    }
}
