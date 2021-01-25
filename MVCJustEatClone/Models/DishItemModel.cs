using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVCJustEatClone.Models
{
    public class DishItemModel
    {
        public Dish Dish { get; set; }
        [Display(Name ="Quantity")]
        public int Quantity { get; set; }
        public int RestaurantId { get; set; }
    }
}
