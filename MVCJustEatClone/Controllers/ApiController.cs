using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVCJustEatClone.Models;
using MVCJustEatClone.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCJustEatClone.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IOrderRepository orderRepository;

        public ApiController(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Post(DishItemModel dishItem)
        {
 
            try
            {
                var order = await orderRepository.GetOrderByRestaurantIdAsync(dishItem.RestaurantId);

                if (order == null)
                {
                    order = new Order()
                    {
                        DateStarted = DateTime.Now,
                        RestaurantId = dishItem.RestaurantId
                    };

                    order.OrderId = await orderRepository.CreateOrderAsync(order);
                }

                var orderItem = new OrderItem()
                {
                    Dish = dishItem.Dish,
                    Quantity = dishItem.Quantity,
                    Price = dishItem.Quantity * dishItem.Dish.Price
                };

                // check to see if theres an existing order item and then update it else add a new one

                if (order.OrderItems.Where(oi => oi.Dish.DishId == dishItem.Dish.DishId).Count() > 0)
                {
                    await orderRepository.UpdateOrderItem(orderItem, order.OrderId);
                }
                else
                {
                    await orderRepository.AddItemToOrderAsync(orderItem, order.OrderId);
                }

                return Ok(order.OrderId);
            }
            catch (Exception)
            {

                throw;
            }


        }

        [HttpDelete("{orderId}/{orderItemId}")]
        public async Task<IActionResult> Delete(int orderId, int orderItemId)
        {
            try
            {
                var results = await orderRepository.RemoveItemFromOrder(orderId, orderItemId);

                if (results > 0)
                {
                    return Ok(results);
                }

                return BadRequest();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
