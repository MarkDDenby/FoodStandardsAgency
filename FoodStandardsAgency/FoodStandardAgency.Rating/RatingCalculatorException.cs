using System;

namespace FoodStandardsAgency.Rating
{
    /// <summary>
    /// Rating calculator exception.
    /// </summary>
    public class RatingCalculatorException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:FoodStandardsAgency.Rating.RatingCalculatorException"/> class.
        /// </summary>
        public RatingCalculatorException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:FoodStandardsAgency.Rating.RatingCalculatorException"/> class.
        /// </summary>
        /// <param name="message">Message.</param>
        public RatingCalculatorException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:FoodStandardsAgency.Rating.RatingCalculatorException"/> class.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="inner">Inner.</param>
        public RatingCalculatorException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
