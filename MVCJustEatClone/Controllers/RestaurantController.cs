using Microsoft.AspNetCore.Mvc;
using MVCJustEatClone.Models;
using MVCJustEatClone.Models.ViewModels;
using MVCJustEatClone.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Session;
using MVCJustEatClone.Extensions;

namespace MVCJustEatClone.Controllers
{
    public class RestaurantController : Controller
    {
        private readonly IRestaurantRepository restaurantRepository;
        private readonly IDishRepository dishRepository;
        private readonly IOrderRepository orderRepository;

        public object Session { get; private set; }

        public RestaurantController(IRestaurantRepository restaurantRepository, IDishRepository dishRepository, IOrderRepository orderRepository)
        {
            this.restaurantRepository = restaurantRepository;
            this.dishRepository = dishRepository;
            this.orderRepository = orderRepository;
        }

        public async Task<IActionResult> Menu(int id)
        {
            var restaurant = await restaurantRepository.GetRestaurantByIdAsync(id);
            var order = new Order();
            order.OrderItems = new List<OrderItem>();

            if (restaurant == null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            var menuCategories = await restaurantRepository.GetMenuCategoriesAsync(id);

            if (User.Claims.Count() > 0)
            {
                var userId = User.GetUserId();
                order = await orderRepository.GetOrdersByUserIdAndRestaurantId(userId, id);
            }

            var viewModel = new RestaurantViewModel()
            {
                Restaurant = restaurant,
                MenuCategories = menuCategories.ToList(),
                Order = order,
                DishItems = new List<DishItemModel>()
                
            };

            foreach (var dish in restaurant.Dishes)
            {
                var dishItem = new DishItemModel() { Dish = dish, Quantity = 0, RestaurantId = id };
                viewModel.DishItems.Add(dishItem);
            };

           
            return View(viewModel);
        }

        
        public async Task<PartialViewResult> OrderPartialView(int orderId)
        {
            var order = await orderRepository.GetOrderByOrderIdAsync(orderId);

            if (order == null)
            {
                order = new Order();
            }

            return PartialView("_OrderSummary", order);
        }

        
        public async Task<PartialViewResult> GetDishModal(int dishId)
        {
            var dish = await restaurantRepository.GetDishByIdAsync(dishId);

            var dishItem = new DishItemModel()
            {
                Dish = dish,
                Quantity = 0,
                RestaurantId = dish.RestaurantId
            };

            return PartialView("_DishItemModal", dishItem);
        }

        

    }
}
