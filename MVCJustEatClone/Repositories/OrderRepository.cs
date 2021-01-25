using Dapper;
using Microsoft.Extensions.Configuration;
using MVCJustEatClone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MVCJustEatClone.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        public OrderRepository(IConfiguration configuration)
        {
            Connection = configuration.GetConnectionString("DefaultConnection");
        }

        public string Connection { get; set; }

        public async Task<int> AddItemToOrderAsync(OrderItem orderItem, int orderId)
        {
            using (var conn = new SqlConnection(Connection))
            {
                var query = $@"INSERT INTO OrderItems (OrderId, DishId, Quantity, Price)
                    VALUES ({orderId}, {orderItem.Dish.DishId}, {orderItem.Quantity}, {orderItem.Price});";
                var rows = await conn.ExecuteAsync(query);
                return rows;
            }

        }

        public async Task<int> CreateOrderAsync(Order order)
        {
            using (var conn = new SqlConnection(Connection))
            {
                var query = $@"INSERT INTO OrderSummary (UserId, DateStarted , RestaurantId)
                    OUTPUT inserted.OrderId
                    VALUES ('T3ST-1D', '{order.DateStarted.ToString("s")}'  ,{order.RestaurantId})";
                var result = await conn.ExecuteScalarAsync<int>(query);
                return result;
            }

        }

        public async Task<Order> GetOrderByRestaurantIdAsync(int restaurantId)
        {
            var order = new Order();
            var orderItems = new List<OrderItem>();

            using (var conn = new SqlConnection(Connection))
            {
                var query = $@"SELECT TOP (1) *
                    FROM OrderSummary
                    WHERE RestaurantId = {restaurantId}
					AND DateFinished IS NULL";
                order = await conn.QueryFirstOrDefaultAsync<Order>(query);
            }

            if (order == null)
            {
                return null;
            }

            using (var conn = new SqlConnection(Connection))
            {
                var query = $@"SELECT OrderItemId, Quantity, OrderItems.Price, Dishes.DishId, DishName
                    FROM OrderItems
                    INNER JOIN Dishes ON OrderItems.DishId = Dishes.DishId
                    WHERE OrderId = {order.OrderId}";
                var data = await conn.QueryAsync<OrderItem, Dish, OrderItem>(query,
                    (orderItem, dish) => {
                        orderItem.Dish = dish;
                        return orderItem;
                    },
                    splitOn: "DishId");
                orderItems = data.ToList();
            }

            order.OrderItems = orderItems;
            order.TotalPrice = order.OrderItems.Sum(items => items.Price);

            return order;
        }

        public async Task RemoveItemFromOrder(int orderItemId)
        {
            using (var conn = new SqlConnection(Connection))
            {
                var deleteQuery = $@"DELETE FROM OrderItems 
                    WHERE OrderItemId = {orderItemId}";

                await conn.ExecuteAsync(deleteQuery);

            }
        }

        public async Task<int> GetRestaurantIdByOrderItemId(int orderItemId)
        {
            using (var conn = new SqlConnection(Connection))
            {
                var query = $@"SELECT TOP (1) OrderSummary.RestaurantId
                    FROM OrderItems
                    INNER JOIN OrderSummary ON OrderItems.OrderId = OrderSummary.OrderId
                    WHERE OrderItems.OrderItemId = {orderItemId}";

                return await conn.QuerySingleAsync<int>(query);
            }
        }

        public async Task<int> UpdateOrderItem(OrderItem orderItem, int orderId)
        {
            using (var conn = new SqlConnection(Connection))
            {
                var query = $@"UPDATE OrderItems SET Quantity = {orderItem.Quantity}
                    WHERE DishId = {orderItem.Dish.DishId}
                    AND OrderId = {orderId};";
                return await conn.ExecuteAsync(query);
            }
        }
    }
}
