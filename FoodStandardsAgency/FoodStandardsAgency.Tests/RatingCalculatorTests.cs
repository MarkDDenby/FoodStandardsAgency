using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using ServiceClient.Contracts;
using ServiceClient.Models;
using FoodStandardsAgency.Rating;
using FoodStandardsAgency.Rating.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Logging;
using Moq;

namespace FoodStandardsAgency.Tests
{
    [TestClass]
    public class RatingCalculatorTests
    {
        Mock<ILogger<RatingCalculator>> mockLogger;
        Mock<IRatingFactory> mockRatingFactory;
        Mock<IFoodStandardAgencyServiceClient> mockServiceClient;

        [TestInitialize]
        public void Setup()
        {
            mockLogger = new Mock<ILogger<RatingCalculator>>();
            mockRatingFactory = new Mock<IRatingFactory>();
            mockServiceClient = new Mock<IFoodStandardAgencyServiceClient>();
        }

        [TestMethod]
        public void RatingCalculator_Throws_ArgumentNullException_When_Given_Null_Service_Client()
        {
            try
            {
                var _sut = new RatingCalculator(null, mockLogger.Object, mockRatingFactory.Object);
            }
            catch (ArgumentNullException ex)
            {
                 Assert.AreEqual("serviceClient", ex.ParamName);
            }
        }

        [TestMethod]
        public void RatingCalculator_Throws_ArgumentNullException_When_Given_Null_Logger()
        {
            try
            {
                var _sut = new RatingCalculator(mockServiceClient.Object, null, mockRatingFactory.Object);
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("logger", ex.ParamName);
            }
        }

        [TestMethod]
        public void RatingCalculator_Throws_ArgumentNullException_When_Given_Null_RatingFactory()
        {
            try
            {
                var _sut = new RatingCalculator(mockServiceClient.Object, mockLogger.Object, null);
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("ratingFactory", ex.ParamName);
            }
        }

        [TestMethod]
        public void RatingCalculator_Calculates_Correctly_When_Given_GB_Style_Ratings()
        {
            // Arrange
            var authority = new Authority() { Name = "Test Authority", RegionName = "TestRegion" };

            var establishments = new Establishments();
            establishments.Items.Add(new Establishment() { LocalAuthorityBusinessId = "1", RatingValue = "1" });
            establishments.Items.Add(new Establishment() { LocalAuthorityBusinessId = "2", RatingValue = "5" });
            establishments.Items.Add(new Establishment() { LocalAuthorityBusinessId = "3", RatingValue = "5" });
            establishments.Items.Add(new Establishment() { LocalAuthorityBusinessId = "4", RatingValue = "1" });
            establishments.Items.Add(new Establishment() { LocalAuthorityBusinessId = "5", RatingValue = "3" });
            establishments.Items.Add(new Establishment() { LocalAuthorityBusinessId = "6", RatingValue = "4" });
            establishments.Items.Add(new Establishment() { LocalAuthorityBusinessId = "7", RatingValue = "4" });
            establishments.Items.Add(new Establishment() { LocalAuthorityBusinessId = "8", RatingValue = "Exempt" });
            establishments.Items.Add(new Establishment() { LocalAuthorityBusinessId = "9", RatingValue = "2" });

            var ratings = new List<AuthorityRating>();
            ratings.Add(new AuthorityRating() { RatingKey = "5", RatingImagePath = "~/FsaImages/England/fhrs_5_en-gb.jpg" });
            ratings.Add(new AuthorityRating() { RatingKey = "4", RatingImagePath = "~/FsaImages/England/fhrs_4_en-gb.jpg" });
            ratings.Add(new AuthorityRating() { RatingKey = "3", RatingImagePath = "~/FsaImages/England/fhrs_3_en-gb.jpg" });
            ratings.Add(new AuthorityRating() { RatingKey = "2", RatingImagePath = "~/FsaImages/England/fhrs_2_en-gb.jpg" });
            ratings.Add(new AuthorityRating() { RatingKey = "1", RatingImagePath = "~/FsaImages/England/fhrs_1_en-gb.jpg" });
            ratings.Add(new AuthorityRating() { RatingKey = "Exempt", RatingImagePath = "~/FsaImages/England/fhrs_exempt_en-gb.jpg" });

            mockServiceClient.Setup(o => o.GetAuthority(1234)).Returns(Task.FromResult(authority));
            mockServiceClient.Setup(o => o.GetEstablishments(1234)).Returns(Task.FromResult(establishments));
            mockRatingFactory.Setup(o => o.GetRatings("TestRegion")).Returns(ratings);

            var _sut = new RatingCalculator(mockServiceClient.Object, mockLogger.Object, mockRatingFactory.Object);
           
            // Act
            var results = _sut.Calculate(1234);

            // Assert
            Assert.AreEqual(6, results.Count);

            // Rating 1
            var ratingOne = results.FirstOrDefault(o => o.RatingKey == "1");
            Assert.IsNotNull(ratingOne);
            Assert.AreEqual(2, ratingOne.RatingCount);
            Assert.AreEqual("22.22", ratingOne.Percentage.ToString("n2"));

            // Rating 2
            var ratingTwo = results.FirstOrDefault(o => o.RatingKey == "2");
            Assert.IsNotNull(ratingTwo);
            Assert.AreEqual(1, ratingTwo.RatingCount);
            Assert.AreEqual("11.11", ratingTwo.Percentage.ToString("n2"));

            // Rating 3
            var ratingThree = results.FirstOrDefault(o => o.RatingKey == "3");
            Assert.IsNotNull(ratingThree);
            Assert.AreEqual(1, ratingThree.RatingCount);
            Assert.AreEqual("11.11", ratingThree.Percentage.ToString("n2"));

            // Rating 4
            var ratingFour = results.FirstOrDefault(o => o.RatingKey == "4");
            Assert.IsNotNull(ratingFour);
            Assert.AreEqual(2, ratingFour.RatingCount);
            Assert.AreEqual("22.22", ratingFour.Percentage.ToString("n2"));

            // Rating 5
            var ratingFive = results.FirstOrDefault(o => o.RatingKey == "5");
            Assert.IsNotNull(ratingFive);
            Assert.AreEqual(2, ratingFive.RatingCount);
            Assert.AreEqual("22.22", ratingFive.Percentage.ToString("n2"));

            // Rating Exempt
            var ratingExempt = results.FirstOrDefault(o => o.RatingKey == "Exempt");
            Assert.IsNotNull(ratingExempt);
            Assert.AreEqual(1, ratingExempt.RatingCount);
            Assert.AreEqual("11.11", ratingExempt.Percentage.ToString("n2"));
        }

        [TestMethod]
        public void RatingCalculator_Calculates_Correctly_When_Given_Scottish_Style_Ratings()
        {
            // Arrange
            var authority = new Authority() { Name = "Test Authority", RegionName = "TestRegion" };

            var establishments = new Establishments();
            establishments.Items.Add(new Establishment() { LocalAuthorityBusinessId = "1", RatingValue = "Pass" });
            establishments.Items.Add(new Establishment() { LocalAuthorityBusinessId = "2", RatingValue = "Pass" });
            establishments.Items.Add(new Establishment() { LocalAuthorityBusinessId = "3", RatingValue = "Improvement Required" });
            establishments.Items.Add(new Establishment() { LocalAuthorityBusinessId = "4", RatingValue = "Improvement Required" });
            establishments.Items.Add(new Establishment() { LocalAuthorityBusinessId = "5", RatingValue = "Improvement Required" });
            establishments.Items.Add(new Establishment() { LocalAuthorityBusinessId = "6", RatingValue = "Pass" });
            establishments.Items.Add(new Establishment() { LocalAuthorityBusinessId = "7", RatingValue = "Improvement Required" });
            establishments.Items.Add(new Establishment() { LocalAuthorityBusinessId = "8", RatingValue = "Improvement Required" });
            establishments.Items.Add(new Establishment() { LocalAuthorityBusinessId = "9", RatingValue = "Pass" });

            var ratings = new List<AuthorityRating>();
            ratings.Add(new AuthorityRating() { RatingKey = "Pass", RatingImagePath = "~/FsaImages/Scotland/fhis_pass.jpg" });
            ratings.Add(new AuthorityRating() { RatingKey = "Improvement Required", RatingImagePath = "~/FsaImages/Scotland/fhis_improvement_required.jpg" });

            mockServiceClient.Setup(o => o.GetAuthority(1234)).Returns(Task.FromResult(authority));
            mockServiceClient.Setup(o => o.GetEstablishments(1234)).Returns(Task.FromResult(establishments));
            mockRatingFactory.Setup(o => o.GetRatings("TestRegion")).Returns(ratings);

            var _sut = new RatingCalculator(mockServiceClient.Object, mockLogger.Object, mockRatingFactory.Object);

            // Act
            var results = _sut.Calculate(1234);

            // Assert
            Assert.AreEqual(2, results.Count);

            // Rating Pass
            var ratingOne = results.FirstOrDefault(o => o.RatingKey == "Pass");
            Assert.IsNotNull(ratingOne);
            Assert.AreEqual(4, ratingOne.RatingCount);
            Assert.AreEqual("44.44", ratingOne.Percentage.ToString("n2"));

            // Rating Improvement Required
            var ratingTwo = results.FirstOrDefault(o => o.RatingKey == "Improvement Required");
            Assert.IsNotNull(ratingTwo);
            Assert.AreEqual(5, ratingTwo.RatingCount);
            Assert.AreEqual("55.56", ratingTwo.Percentage.ToString("n2"));
        }
    }
}