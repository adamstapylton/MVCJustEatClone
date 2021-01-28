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
    public class RestaurantRepository : IRestaurantRepository
    {
        private string Connection;

        public RestaurantRepository(IConfiguration configuration)
        {
            Configuration = configuration;
            Connection = Configuration.GetConnectionString("DefaultConnection");
        }

        public IConfiguration Configuration { get; }

        public IEnumerable<Category> GetCategories()
        {
            using (var conn = new SqlConnection(Connection))
            {
                var query = "SELECT TOP (6) * FROM Categories";
                return conn.Query<Category>(query);
            }
        }

        public async Task<Dish> GetDishByIdAsync(int dishId)
        {
            using (var conn = new SqlConnection(Connection))
            {
                var query = $"SELECT * FROM Dishes WHERE DishId = {dishId}";
                return await conn.QueryFirstOrDefaultAsync<Dish>(query);
            }
        }

        public IEnumerable<Dish> GetMenuByRestaurantId(int id)
        {
            using (var conn = new SqlConnection(Connection))
            {
                var query = $@"  SELECT DishId, DishName, Price, MenuCategories.MenuCategoryId, MenuCategoryName
                FROM Restaurants
                INNER JOIN Dishes ON Restaurants.RestaurantId = Dishes.RestaurantId
                INNER JOIN MenuCategories ON Dishes.MenuCategoryId = MenuCategories.MenuCategoryId
				WHERE Restaurants.RestaurantId = {id}";

                return conn.Query<Dish, MenuCategory, Dish>(query, 
                    (dish, category) => {
                        dish.Category = category;
                        return dish;
                     },
                    splitOn: "MenuCategoryId");
            }
        }

        public async Task<IEnumerable<MenuCategory>> GetMenuCategoriesAsync(int id)
        {
            using (var conn = new SqlConnection(Connection))
            {
                var query = $@"SELECT DISTINCT MenuCategories.MenuCategoryId, MenuCategoryName
                FROM Restaurants
                INNER JOIN Dishes ON Restaurants.RestaurantId = Dishes.RestaurantId
                INNER JOIN MenuCategories ON Dishes.MenuCategoryId = MenuCategories.MenuCategoryId
				WHERE Restaurants.RestaurantId = {id}";

                return await conn.QueryAsync<MenuCategory>(query);
            }
        }

        public async Task<IEnumerable<Category>> GetRestarauntCategoriesAsync(int id)
        {
            using (var conn = new SqlConnection(Connection))
            {
                var query = $@"  SELECT Categories.CategoryId, CategoryName
                FROM Restaurants
                INNER JOIN RestaurantsCategories ON Restaurants.RestaurantId = RestaurantsCategories.RestaurantId
                INNER JOIN Categories ON RestaurantsCategories.CategoryId = Categories.CategoryId
                WHERE Restaurants.RestaurantId = {id}";
                return await conn.QueryAsync<Category>(query);
            }
        }

        public async Task<Restaurant> GetRestaurantByIdAsync(int id)
        {
            var restaurants = await GetRestaurantsAsync();
            var restaurant = restaurants.Where(r => r.RestaurantId == id).FirstOrDefault(); 

            if (restaurant != null)
            {
                return restaurant;
            }

            return null;
        }

        public async Task<IEnumerable<Restaurant>> GetRestaurantsAsync()
        {
            var restaurants = new List<Restaurant>();

            using (var conn = new SqlConnection(Connection))
            {
                var restaurantsQuery = @"SELECT * FROM Restaurants";
                var results = await conn.QueryAsync<Restaurant>(restaurantsQuery);
                restaurants = results.ToList();
            }

            foreach (var restaurant in restaurants)
            {
                restaurant.Dishes = GetMenuByRestaurantId(restaurant.RestaurantId).ToList();
                var categories = await GetRestarauntCategoriesAsync(restaurant.RestaurantId);
                restaurant.Categories = categories.ToList();
            }

            return restaurants;

        }

        public async  Task<IEnumerable<Restaurant>> SearchRestaurantsAsync(string search)
        {
            var restaurants = await GetRestaurantsAsync();
            return restaurants.Where(r => r.RestaurantName.ToLower().Contains(search.ToLower()));
        }
    }
}
