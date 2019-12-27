using System;
using System.Collections.Generic;

namespace FoodStandardsAgency.Rating.Contracts
{
    public interface IRatingFactory
    {
        List<AuthorityRating> GetRatings(string regionName);
    }
}
