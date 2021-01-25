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
    public class DishRepository : IDishRepository
    {
        private readonly IConfiguration configuration;

        public DishRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            Connection = configuration.GetConnectionString("DefaultConnection");
        }

        private string Connection { get; set; }

        public async Task<Dish> GetDishByIdAsync(int dishId)
        {
            var query = $"SELECT * FROM Dishes WHERE DishId = {dishId}";

            using (var conn = new SqlConnection(Connection))
            {
                var dish = await conn.QuerySingleAsync<Dish>(query);
                return dish;
            }
        }
    }
}
