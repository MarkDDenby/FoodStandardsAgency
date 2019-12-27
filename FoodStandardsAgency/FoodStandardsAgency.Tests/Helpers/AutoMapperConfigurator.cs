
using AutoMapper;
using FoodStandardsAgency.AutoMapper;

namespace FoodStandardsAgency.Tests.Helpers
{
    /// <summary>
    /// Auto mapper configurator helps us create one static instance of the automapper for use in tests
    /// </summary>
    public static class AutoMapperConfigurator
    {
        static object _isMappinginitialized = false;

        public static void Setup()
        {
            // Prevent the auto mapper from being initialized more than once 
            lock (_isMappinginitialized)
            {
                if ((bool)_isMappinginitialized == false)
                {
                    Mapper.Initialize(m => m.AddProfile<MapperProfile>());
                    _isMappinginitialized = true;
                }
            }
        }
    }
}
