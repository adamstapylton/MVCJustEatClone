using Microsoft.AspNetCore.Mvc;
using MVCJustEatClone.Models;
using MVCJustEatClone.Models.ViewModels;
using MVCJustEatClone.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Session;

namespace MVCJustEatClone.Controllers
{
    public class RestaurantController : Controller
    {
        private readonly IRestaurantRepository restaurantRepository;
        private readonly IDishRepository dishRepository;
        private readonly IOrderRepository orderRepository;

        public RestaurantController(IRestaurantRepository restaurantRepository, IDishRepository dishRepository, IOrderRepository orderRepository)
        {
            this.restaurantRepository = restaurantRepository;
            this.dishRepository = dishRepository;
            this.orderRepository = orderRepository;
        }

        public async Task<IActionResult> Menu(int id)
        {
            var restaurant = await restaurantRepository.GetRestaurantByIdAsync(id);

            if (restaurant == null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            var menuCategories = await restaurantRepository.GetMenuCategoriesAsync(id);
            
            var order = await orderRepository.GetOrderByRestaurantIdAsync(id);

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

           
            return View("Menu", viewModel);
        }

        
        //public async Task<RedirectResult> AddToOrder(DishItemModel dishItem)
        //{
        //    var order = await orderRepository.GetOrderByRestaurantIdAsync(dishItem.RestaurantId);

        //    if (order == null)
        //    {
        //        order = new Order()
        //        {
        //            DateStarted = DateTime.Now,
        //            RestaurantId = dishItem.RestaurantId
        //        };

        //        order.OrderId = await orderRepository.CreateOrderAsync(order);
        //    }

        //    var orderItem = new OrderItem()
        //    {
        //        Dish = dishItem.Dish,
        //        Quantity = dishItem.Quantity,
        //        Price = dishItem.Quantity * dishItem.Dish.Price
        //    };

        //    // check to see if theres an existing order item and then update it else add a new one

        //    if (order.OrderItems.Where(oi => oi.Dish.DishId == dishItem.Dish.DishId).Count() > 0)
        //    {
        //        await orderRepository.UpdateOrderItem(orderItem, order.OrderId);
        //    }
        //    else
        //    {
        //        await orderRepository.AddItemToOrderAsync(orderItem, order.OrderId);
        //    }
            

        //    return Redirect($"/Restaurant/Menu/{dishItem.RestaurantId}");

        //}

        //public async Task<RedirectResult> RemoveItem(int itemId)
        //{
        //    var restaurantId = await orderRepository.GetRestaurantIdByOrderItemId(itemId);

        //    await orderRepository.RemoveItemFromOrder(itemId);    

        //     return Redirect($"/Restaurant/Menu/{restaurantId}");
        //}

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
