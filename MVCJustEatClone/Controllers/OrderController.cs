using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCJustEatClone.Extensions;
using MVCJustEatClone.Models.ViewModels;
using MVCJustEatClone.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCJustEatClone.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderRepository orderRepository;
        private readonly IRestaurantRepository restaurantRepository;

        public OrderController(IOrderRepository orderRepository, IRestaurantRepository restaurantRepository)
        {
            this.orderRepository = orderRepository;
            this.restaurantRepository = restaurantRepository;
        }

        public async Task<IActionResult> Checkout(int orderId)
        {
            var UserId = User.GetUserId();

            var order = await orderRepository.GetOrderByOrderIdAsync(orderId);
            var restaurant = await restaurantRepository.GetRestaurantByIdAsync(order.RestaurantId);

            var checkoutViewModel = new CheckoutViewModel()
            {
                Order = order,
                Restaurant = restaurant 
            };

            ViewBag.ShowCheckout = false;

            return View(checkoutViewModel);
        }
    }
}
