using MVCJustEatClone.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCJustEatClone.Repositories
{
    public interface IOrderRepository
    {
        Task<int> CreateOrderAsync(Order order);
        Task<Order> GetOrderByRestaurantIdAsync(int restaurantId);
        Task<int> AddItemToOrderAsync(OrderItem orderItem, int orderId);
        Task RemoveItemFromOrder(int orderItemId);
        Task<int> GetRestaurantIdByOrderItemId(int orderItemId);
        Task<int> UpdateOrderItem(OrderItem orderItem, int orderId);

    }
}
