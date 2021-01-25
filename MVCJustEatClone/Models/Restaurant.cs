using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCJustEatClone.Models
{
    public class Restaurant
    {
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public List<Dish> Dishes { get; set; }
        public List<Category> Categories { get; set; }
        public string PrimaryImageUrl { get; set; }
        public string SecondaryImageUrl { get; set; }
        public bool CurrentlyOpen { get; set; }
        public double DeliveryCharge { get; set; }
        public double MinOrderAmount { get; set; }
        public double Rating { get; set; }
    }
}
