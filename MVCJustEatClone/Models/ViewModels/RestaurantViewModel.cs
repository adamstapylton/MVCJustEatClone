using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCJustEatClone.Models.ViewModels
{
    public class RestaurantViewModel
    {
        public Restaurant Restaurant { get; set; }
        public List<MenuCategory> MenuCategories { get; set; }
        public Order Order { get; set; }
        public List<DishItemModel> DishItems { get; set; }
        public string Test { get; set; }
    }
}
