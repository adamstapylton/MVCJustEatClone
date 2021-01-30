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
            var parameters = new 
            {
                OrderId = orderId,
                DishId = orderItem.Dish.DishId,
                Quantity = orderItem.Quantity,
                Price = orderItem.Price
            };

            using (var conn = new SqlConnection(Connection))
            {
                var query = $@"INSERT INTO OrderItems (OrderId, DishId, Quantity, Price)
                    VALUES (@OrderId, @DishId, @Quantity, @Price);";
                var rows = await conn.ExecuteAsync(query, parameters);
                return rows;
            }

        }

        public async Task<int> CreateOrderAsync(Order order)
        {
            var parameters = new
            {
                DateStarted = order.DateStarted.ToString("s") ,
                RestaurantId = order.RestaurantId
            };

              
            using (var conn = new SqlConnection(Connection))
            {
                var query = $@"INSERT INTO OrderSummary ( DateStarted , RestaurantId)
                    OUTPUT inserted.OrderId
                    VALUES ( '@DateStarted'  ,@RestaurantId)";
                var result = await conn.ExecuteScalarAsync<int>(query, parameters);
                return result;
            }

        }

     //   public async Task<Order> GetOrderByRestaurantIdAsync(int restaurantId)
     //   {
     //       var order = new Order();
     //       var orderItems = new List<OrderItem>();
     //       var dictionary = new Dictionary<string, int>() 
     //       {
     //           { "@RestaurantId", restaurantId},
     //           { "@OrderId", order.OrderId }
     //       };

     //       var parameters = new DynamicParameters(dictionary);

     //       using (var conn = new SqlConnection(Connection))
     //       {
     //           var query = $@"SELECT TOP (1) *
     //               FROM OrderSummary
     //               WHERE RestaurantId = @RestaurantId
					//AND DateFinished IS NULL";
     //           order = await conn.QueryFirstOrDefaultAsync<Order>(query, parameters);
     //       }

     //       if (order == null)
     //       {
     //           return null;
     //       }

     //       using (var conn = new SqlConnection(Connection))
     //       {
     //           var query = $@"SELECT OrderItemId, Quantity, OrderItems.Price, Dishes.DishId, DishName
     //               FROM OrderItems
     //               INNER JOIN Dishes ON OrderItems.DishId = Dishes.DishId
     //               WHERE OrderId = {order.OrderId}";
     //           var data = await conn.QueryAsync<OrderItem, Dish, OrderItem>(query,
     //               (orderItem, dish) => {
     //                   orderItem.Dish = dish;
     //                   return orderItem;
     //               },
     //               splitOn: "DishId");
     //           orderItems = data.ToList();
     //       }

     //       order.OrderItems = orderItems;
     //       order.TotalPrice = order.OrderItems.Sum(items => items.Price);

     //       return order;
     //   }

        public async Task<int> RemoveItemFromOrder(int orderId, int orderItemId)
        {
            var parameters = new
            {
                OrderId = orderId ,
                OrderItemId = orderItemId 
            };
     

            using (var conn = new SqlConnection(Connection))
            {
                var deleteQuery = $@"DELETE FROM OrderItems 
                    WHERE OrderItemId = @OrderItemId
                    AND OrderId = @OrderId";

                return await conn.ExecuteAsync(deleteQuery, parameters);

            }
        }

        public async Task<int> GetRestaurantIdByOrderItemId(int orderItemId)
        {
            var parameters = new { OrderItemId = orderItemId };

            using (var conn = new SqlConnection(Connection))
            {
                var query = $@"SELECT TOP (1) OrderSummary.RestaurantId
                    FROM OrderItems
                    INNER JOIN OrderSummary ON OrderItems.OrderId = OrderSummary.OrderId
                    WHERE OrderItems.OrderItemId = @OrderItemId";

                return await conn.QuerySingleAsync<int>(query, parameters);
            }
        }

        public async Task<int> UpdateOrderItem(OrderItem orderItem, int orderId)
        {
            var parameters = new
            {
                Quantity = orderItem.Quantity,
                Price = orderItem.Price,
                DishId = orderItem.Dish.DishId,
                OrderId = orderId
            };

            using (var conn = new SqlConnection(Connection))
            {
                var query = $@"UPDATE OrderItems 
                    SET Quantity = @Quantity, Price = @Price
                    WHERE DishId = @DishId
                    AND OrderId = @OrderId;";
                return await conn.ExecuteAsync(query, parameters);
            }
        }

        public async Task<Order> GetOrderByOrderIdAsync(int orderId)
        {
            var order = new Order();
            var orderItems = new List<OrderItem>();

            var parameters = new { OrderId = orderId };

            using (var conn = new SqlConnection(Connection))
            {
                var query = $@"SELECT *
                    FROM OrderSummary
                    WHERE OrderId = @OrderId";
                order =  await conn.QueryFirstAsync<Order>(query, parameters);

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
            };

            order.OrderItems = orderItems;
            order.TotalPrice = order.OrderItems.Sum(items => items.Price);

            return order;
            //return await GetOrderByRestaurantIdAsync(order.RestaurantId);
        }

        public async Task<Order> GetOrdersByUserIdAndRestaurantId(int userId, int restaurantId)
        {
            var order = new Order();

            var parameters = new 
            { 
                UserId = userId,
                RestaurantId = restaurantId
            };

            using (var conn = new SqlConnection(Connection))
            {
                var query = @"  SELECT *
                  FROM OrderSummary
                  WHERE UserId = @UserId
                  AND RestaurantId = @RestaurantId";
                order = await conn.QueryFirstAsync<Order>(query, parameters);
            }

            order.OrderItems = await GetOrderItems(order.OrderId);

            return order;
        }

        public async Task<List<OrderItem>> GetOrderItems(int orderId)
        {
            var orderItems = new List<OrderItem>();

           // var parameters = new { OrderId = orderId };

            using (var conn = new SqlConnection(Connection))
            {
                var query = $@"SELECT OrderItemId, Quantity, OrderItems.Price, Dishes.DishId, DishName
                    FROM OrderItems
                    INNER JOIN Dishes ON OrderItems.DishId = Dishes.DishId
                    WHERE OrderId = {orderId}";
                var data = await conn.QueryAsync<OrderItem, Dish, OrderItem>(query,
                    (orderItem, dish) => {
                        orderItem.Dish = dish;
                        return orderItem;
                    },
                    splitOn: "DishId");
                orderItems = data.ToList();
            };

            return orderItems;
        }
    }
}
