using MVCJustEatClone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCJustEatClone.Repositories
{
    public interface IDishRepository
    {
        Task<Dish> GetDishByIdAsync(int dishId);
    }
}
