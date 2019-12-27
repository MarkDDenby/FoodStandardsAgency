using System;
using System.Collections.Generic;

namespace FoodStandardsAgency.Rating.Contracts
{
    public interface IRatingCalculator
    {
        List<AuthorityRating> Calculate(int authorityId);
    }
}
