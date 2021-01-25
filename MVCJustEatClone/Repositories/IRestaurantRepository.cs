using MVCJustEatClone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCJustEatClone.Repositories
{
    public interface IRestaurantRepository
    {
        Task<IEnumerable<Restaurant>> GetRestaurantsAsync();
        Task<Restaurant> GetRestaurantByIdAsync(int id);
        IEnumerable<Category> GetCategories();
        Task<IEnumerable<Restaurant>> SearchRestaurantsAsync(string search);
        IEnumerable<Dish> GetMenuByRestaurantId(int id);
        Task<IEnumerable<Category>> GetRestarauntCategoriesAsync(int id);
        Task<IEnumerable<MenuCategory>> GetMenuCategoriesAsync(int id);

    }
}
