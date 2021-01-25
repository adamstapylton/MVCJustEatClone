using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVCJustEatClone.Models;
using MVCJustEatClone.Models.ViewModels;
using MVCJustEatClone.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MVCJustEatClone.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRestaurantRepository restaurantRepository;

        public HomeController(ILogger<HomeController> logger, IRestaurantRepository restaurantRepository)
        {
            _logger = logger;
            this.restaurantRepository = restaurantRepository;
        }

        public async Task<IActionResult> Index()
        {
            var restaurants = await restaurantRepository.GetRestaurantsAsync();

            var viewModel = new HomeViewModel()
            {
                
                Restaurants = restaurants.ToList(),
                Categories = restaurantRepository.GetCategories().ToList(),
                SearchUsed = false
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult NotFound()
        {
            return View();
        }

        public async Task<IActionResult> SearchRestaurants(HomeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var allRestaurants = await restaurantRepository.GetRestaurantsAsync();
                viewModel.Restaurants = allRestaurants.ToList();
                viewModel.Categories = restaurantRepository.GetCategories().ToList();
                viewModel.SearchUsed = false;
                return View("Index", viewModel);
            }

            var restaurants = await restaurantRepository.SearchRestaurantsAsync(viewModel.Search);
            viewModel.Restaurants = restaurants.ToList();
            viewModel.Categories = restaurantRepository.GetCategories().ToList();
            viewModel.SearchUsed = true;

            return View("Index", viewModel);
        }

    }
}
