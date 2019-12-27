
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using FoodStandardsAgency.Rating.Contracts;
using ServiceClient.Contracts;
using ServiceClient.Models;
using FoodStandardsAgency.Controllers;
using FoodStandardsAgency.Models;
using FoodStandardsAgency.Tests.Helpers;
using FoodStandardsAgency.Rating;
using Moq;

namespace FoodStandardsAgency.Tests
{
    [TestClass]
    public class HomeControllerTests
    {
        Mock<ILogger<HomeController>> mockLogger;
        Mock<IRatingFactory> mockRatingFactory;
        Mock<IFoodStandardAgencyServiceClient> mockServiceClient;
        Mock<IMemoryCache> mockMemoryCache;
        Mock<IRatingCalculator> mockRatingCalculator;
        Mock<ICacheEntry> mockCacheEntry;

        [TestInitialize]
        public void Setup()
        {
            mockLogger = new Mock<ILogger<HomeController>>();
            mockRatingFactory = new Mock<IRatingFactory>();
            mockServiceClient = new Mock<IFoodStandardAgencyServiceClient>();
            mockMemoryCache = new Mock<IMemoryCache>();
            mockRatingCalculator = new Mock<IRatingCalculator>();
            mockCacheEntry = new Mock<ICacheEntry>();
            mockMemoryCache.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(mockCacheEntry.Object);

            AutoMapperConfigurator.Setup();
        }

        [TestMethod]
        public void HomeController_Index_Returns_Correctly_Populated_ViewModel()
        {
            // Arrange 
            var authorities = new Authorities();
            authorities.Items.Add(new Authority() { Name = "Authority 1", FriendlyName = "Friendly Authority 1", LocalAuthorityId = 1, LocalAuthorityIdCode = "1" });
            authorities.Items.Add(new Authority() { Name = "Authority 2", FriendlyName = "Friendly Authority 2", LocalAuthorityId = 2, LocalAuthorityIdCode = "2" });
            authorities.Items.Add(new Authority() { Name = "Authority 3", FriendlyName = "Friendly Authority 3", LocalAuthorityId = 3, LocalAuthorityIdCode = "3" });
            authorities.Items.Add(new Authority() { Name = "Authority 4", FriendlyName = "Friendly Authority 4", LocalAuthorityId = 4, LocalAuthorityIdCode = "4" });

            mockServiceClient.Setup(o => o.GetAllAuthorities()).Returns(Task.FromResult(authorities));

            // Act
            var controller = new HomeController(mockLogger.Object, mockServiceClient.Object, mockMemoryCache.Object, Mapper.Instance, mockRatingCalculator.Object);
            var result = controller.Index() as ViewResult;
            var viewModel = result.Model as HomeControllerViewModel;

            // Assert
            Assert.IsNotNull(viewModel);
            Assert.AreEqual(4, viewModel.Authorities.Count());
            Assert.AreEqual(4, viewModel.AuthorityListItems.Count());

            Assert.AreEqual("Friendly Authority 1", viewModel.Authorities[0].Name);
            Assert.AreEqual(1, viewModel.Authorities[0].Id);

            Assert.AreEqual("Friendly Authority 2", viewModel.Authorities[1].Name);
            Assert.AreEqual(2, viewModel.Authorities[1].Id);

            Assert.AreEqual("Friendly Authority 3", viewModel.Authorities[2].Name);
            Assert.AreEqual(3, viewModel.Authorities[2].Id);

            Assert.AreEqual("Friendly Authority 4", viewModel.Authorities[3].Name);
            Assert.AreEqual(4, viewModel.Authorities[3].Id);
        }

        [TestMethod]
        public void HomeController_GetAuthorityRating_Returns_Correctly_Populated_ViewModel()
        {
            // Arrange
            var authorities = new Authorities();
            authorities.Items.Add(new Authority() { Name = "Authority 1", FriendlyName = "Friendly Authority 1", LocalAuthorityId = 1, LocalAuthorityIdCode = "1" });
            authorities.Items.Add(new Authority() { Name = "Authority 2", FriendlyName = "Friendly Authority 2", LocalAuthorityId = 2, LocalAuthorityIdCode = "2" });
            authorities.Items.Add(new Authority() { Name = "Authority 3", FriendlyName = "Friendly Authority 3", LocalAuthorityId = 3, LocalAuthorityIdCode = "3" });
            authorities.Items.Add(new Authority() { Name = "Authority 4", FriendlyName = "Friendly Authority 4", LocalAuthorityId = 4, LocalAuthorityIdCode = "4" });

            mockServiceClient.Setup(o => o.GetAllAuthorities()).Returns(Task.FromResult(authorities));

            var calculatorResults = new List<AuthorityRating>();
            calculatorResults.Add(new AuthorityRating() { RatingKey = "1", RatingCount = 1, Percentage = 11, RatingImagePath = "None 1"});
            calculatorResults.Add(new AuthorityRating() { RatingKey = "2", RatingCount = 2, Percentage = 22, RatingImagePath = "None 2"});
            calculatorResults.Add(new AuthorityRating() { RatingKey = "3", RatingCount = 3, Percentage = 33, RatingImagePath = "None 3"});
            calculatorResults.Add(new AuthorityRating() { RatingKey = "4", RatingCount = 4, Percentage = 44, RatingImagePath = "None 4"});

            mockRatingCalculator.Setup(x => x.Calculate(2)).Returns(calculatorResults);

            // Act
            var controller = new HomeController(mockLogger.Object, mockServiceClient.Object, mockMemoryCache.Object, Mapper.Instance, mockRatingCalculator.Object);
            var result = controller.GetAuthorityRating(2) as PartialViewResult;
            var viewModel = result.Model as HomeControllerViewModel;

            // Assert
            Assert.IsNotNull(viewModel);

            // Selected Authority Properties
            Assert.AreEqual(2, viewModel.Id);
            Assert.AreEqual("Friendly Authority 2", viewModel.Name);

            Assert.AreEqual(4, viewModel.AuthorityRating.Count());

            // Rating 1
            Assert.AreEqual("1", viewModel.AuthorityRating[0].RatingKey);
            Assert.AreEqual(1, viewModel.AuthorityRating[0].Rating);
            Assert.AreEqual(11, viewModel.AuthorityRating[0].Percentage);
            Assert.AreEqual("None 1", viewModel.AuthorityRating[0].RatingImagePath);

            // Rating 2
            Assert.AreEqual("2", viewModel.AuthorityRating[1].RatingKey);
            Assert.AreEqual(2, viewModel.AuthorityRating[1].Rating);
            Assert.AreEqual(22, viewModel.AuthorityRating[1].Percentage);
            Assert.AreEqual("None 2", viewModel.AuthorityRating[1].RatingImagePath);

            // Rating 3
            Assert.AreEqual("3", viewModel.AuthorityRating[2].RatingKey);
            Assert.AreEqual(3, viewModel.AuthorityRating[2].Rating);
            Assert.AreEqual(33, viewModel.AuthorityRating[2].Percentage);
            Assert.AreEqual("None 3", viewModel.AuthorityRating[2].RatingImagePath);

            // rating 4
            Assert.AreEqual("4", viewModel.AuthorityRating[3].RatingKey);
            Assert.AreEqual(4, viewModel.AuthorityRating[3].Rating);
            Assert.AreEqual(44, viewModel.AuthorityRating[3].Percentage);
            Assert.AreEqual("None 4", viewModel.AuthorityRating[3].RatingImagePath);
        }

        [TestMethod]
        public void HomeController_Throws_ArgumentNullException_When_Given_Null_Logger()
        {
            try
            {
                var _sut = new HomeController(null, mockServiceClient.Object, mockMemoryCache.Object, Mapper.Instance, mockRatingCalculator.Object);
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("logger", ex.ParamName);
            }
        }

        [TestMethod]
        public void HomeController_Throws_ArgumentNullException_When_Given_Null_ServiceClient()
        {
            try
            {
                var _sut = new HomeController(mockLogger.Object, null, mockMemoryCache.Object, Mapper.Instance, mockRatingCalculator.Object);
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("serviceClient", ex.ParamName);
            }
        }

        [TestMethod]
        public void HomeController_Throws_ArgumentNullException_When_Given_Null_MemoryCache()
        {
            try
            {
                var _sut = new HomeController(mockLogger.Object, mockServiceClient.Object, null, Mapper.Instance, mockRatingCalculator.Object);
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("memoryCache", ex.ParamName);
            }
        }

        [TestMethod]
        public void HomeController_Throws_ArgumentNullException_When_Given_Null_Mapper()
        {
            try
            {
                var _sut = new HomeController(mockLogger.Object, mockServiceClient.Object, mockMemoryCache.Object, null, mockRatingCalculator.Object);
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("mapper", ex.ParamName);
            }
        }

        [TestMethod]
        public void HomeController_Throws_ArgumentNullException_When_Given_Null_RatingCalculator()
        {
            try
            {
                var _sut = new HomeController(mockLogger.Object, mockServiceClient.Object, mockMemoryCache.Object, Mapper.Instance, null);
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("calculator", ex.ParamName);
            }
        }
    }
}