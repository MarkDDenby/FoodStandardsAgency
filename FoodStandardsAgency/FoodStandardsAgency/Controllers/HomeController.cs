using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using AutoMapper;
using FoodStandardsAgency.Models;
using ServiceClient.Contracts;
using ServiceClient.Models;
using FoodStandardsAgency.Rating;
using FoodStandardsAgency.Rating.Contracts;
using ServiceClient;

namespace FoodStandardsAgency.Controllers
{
    public class HomeController : Controller
    {
        private ILogger _logger;
        private IFoodStandardAgencyServiceClient _serviceClient;
        private IMemoryCache _cache;
        private IMapper _mapper;
        private IRatingCalculator _calculator;
        private const string _cacheAuthorityKey = "authority-results-cache";

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:FoodStandardsAgency.Controllers.HomeController"/> class.
        /// </summary>
        /// <param name="logger">.Net Core Logger</param>
        /// <param name="serviceClient">Service client used to communicate with the FSA service</param>
        /// <param name="memoryCache">.Net Core Memory cache mechanism</param>
        /// <param name="mapper">AutoMapper instance</param>
        /// <param name="calculator">Calcuator used to calculate the ratings</param>
        public HomeController(ILogger<HomeController> logger, IFoodStandardAgencyServiceClient serviceClient, IMemoryCache memoryCache, IMapper mapper, IRatingCalculator calculator)
        {
            if(logger == null) 
            {
                throw new ArgumentNullException(nameof(logger));
            }
            if(serviceClient == null)
            {
                throw new ArgumentNullException(nameof(serviceClient));
            }
            if (memoryCache == null)
            {
                throw new ArgumentNullException(nameof(memoryCache));
            }
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }
            if(calculator == null)
            {
                throw new ArgumentNullException(nameof(calculator));
            }

            _logger = logger;
            _serviceClient = serviceClient;
            _cache = memoryCache;
            _mapper = mapper;
            _calculator = calculator;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("HomeController - Index");

            return View(this.InitialiseViewModel()); // return a view with a view model that contains a list of authorities.
        }

        public PartialViewResult GetAuthorityRating(int authorityId)
        {
            _logger.LogInformation(String.Format("HomeController - GetAuthorityRating For AuthorityId {0})", authorityId));

            HomeControllerViewModel viewModel = this.InitialiseViewModel();
            List<AuthorityRating> ratingResults = new List<AuthorityRating>();
            try
            {
                ratingResults = this._calculator.Calculate(authorityId); // call the calculator with the given authority.
            }
            catch (RatingCalculatorException ex)
            {
                _logger.LogError("An unexpected error has occured " + ex.Message);
                RedirectToAction("Home", "Error"); // an error has occured, redirect to the error page.
            }

            // update the view model with the rating results
            viewModel.AuthorityRating = Mapper.Map<List<RatingModel>>(ratingResults);

            // populate the authority details on the view model
            AuthorityModel currentAuthority = viewModel.Authorities.FirstOrDefault(o => o.Id == authorityId);
            viewModel.Id = currentAuthority.Id;
            viewModel.Name = currentAuthority.Name;
            return PartialView("AuthorityRatingPartial", viewModel);
        }

        private HomeControllerViewModel InitialiseViewModel()
        {
            // populate the view model with a list of authorites
            HomeControllerViewModel viewModel = new HomeControllerViewModel();
            try
            {
                List<Authority> results = this.GetAuthorities();
                viewModel.Authorities = Mapper.Map<List<AuthorityModel>>(results);
            }
            catch (ServiceException ex)
            {
                _logger.LogError("An unexpected error has occured " + ex.Message);
                RedirectToAction("Home", "Error");
            }
            return viewModel;
        }

        /// <summary>
        /// Gets a list of authorities.
        /// </summary>
        /// <returns>List of authorities direct from service or from cache</returns>
        private List<Authority> GetAuthorities()
        {
            List<Authority> results = new List<Authority>();

            // attempt to get data from cache
            if (_cache.TryGetValue(_cacheAuthorityKey, out List<Authority> cachedAuthorities))
            {
                _logger.LogInformation("Retrieving Authorities From Cached Data");   
                results = cachedAuthorities;
            }
            else
            {
                // get data if not found in cache and update cache, setting expiration to 1 day.
                _logger.LogInformation("Retrieving Authorities Directly From FSA Service");  
                results = this.GetAllAuthoritiesAsync().Result.Items;
                _cache.Set(_cacheAuthorityKey, results);
                _cache.Set(_cacheAuthorityKey, results, TimeSpan.FromDays(1));
            }

            _logger.LogInformation(String.Format("Returned {0} Authorities From GetAuthorities", results.Count()));

            return results;
        }

        /// <summary>
        /// calls the service to get a list of authorities
        /// </summary>
        /// <returns>Task with a Authorities result.</returns>
        private async Task<Authorities> GetAllAuthoritiesAsync()
        {
            return await _serviceClient.GetAllAuthorities();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
