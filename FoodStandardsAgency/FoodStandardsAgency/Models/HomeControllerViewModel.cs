using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FoodStandardsAgency.Models
{
    /// <summary>
    /// Home controller view model.
    /// </summary>
    public class HomeControllerViewModel
    {
        public HomeControllerViewModel()
        {
            this.Authorities = new List<AuthorityModel>();
            this.AuthorityRating = new List<RatingModel>();
        }

        public int Id { get; set; }                             // Current Authority Id
        public string Name { get; set; }                        // Current Authority Name
        public List<AuthorityModel> Authorities { get; set; }   // List of Authorities
        public List<RatingModel> AuthorityRating { get; set; }  // List of Ratings

        /// <summary>
        /// returns the list of authorities wrapped in a selectlist
        /// </summary>
        /// <value>The authority list items.</value>
        public IEnumerable<SelectListItem> AuthorityListItems
        {
            get
            {
                return new SelectList(this.Authorities, "Id", "Name");
            }
        }
    }
}

