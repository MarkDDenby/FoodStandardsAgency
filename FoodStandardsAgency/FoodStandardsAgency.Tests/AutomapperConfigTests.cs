using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoMapper;
using ServiceClient.Models;
using FoodStandardsAgency.Models;
using FoodStandardsAgency.Rating;
using FoodStandardsAgency.Tests.Helpers;

namespace FoodStandardsAgency.Tests
{
    [TestClass]
    public class AutomapperConfigTests
    {
        [TestInitialize]
        public void Setup()
        {
            AutoMapperConfigurator.Setup();
        }

        [TestMethod]
        public void AutoMapper_Configuration_IsValid()
        {
            Mapper.AssertConfigurationIsValid();
        }

        [TestMethod]
        public void AutoMapper_Authority_Map_To_AuthorityModel_IsCorrect()
        {
            // Arrange
            Authority source = new Authority();
            source.LocalAuthorityId = 1;
            source.LocalAuthorityIdCode = "IdCode";
            source.Name = "Test Authority";
            source.FriendlyName = "Test Friendly";
            source.RegionName = "Test Region";
            AuthorityModel dest;

            // Act
            dest = Mapper.Map<AuthorityModel>(source);

            // Assert
            Assert.AreEqual(source.FriendlyName, dest.Name);
            Assert.AreEqual(source.LocalAuthorityId, dest.Id);
        }

        [TestMethod]
        public void AutoMapper_Rating_Map_To_RatingModel_IsCorrect()
        {
            // Arrange

            AuthorityRating source = new AuthorityRating();
            source.RatingKey = "Rating Key";
            source.RatingCount = 99;
            source.RatingImagePath = "Image Path";
            source.Percentage = 100;
            RatingModel dest;

            // Act
            dest = Mapper.Map<RatingModel>(source);

            // Assert
            Assert.AreEqual(source.Percentage, dest.Percentage);
            Assert.AreEqual(source.RatingCount, dest.Rating);
            Assert.AreEqual(source.RatingImagePath, dest.RatingImagePath);
            Assert.AreEqual(source.RatingKey, dest.RatingKey);
        }
    }
}
