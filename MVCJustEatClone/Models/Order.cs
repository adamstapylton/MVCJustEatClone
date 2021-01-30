using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVCJustEatClone.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        [Display(Name = "Total")]
        public double TotalPrice { get; set; }
        public DateTime DateStarted { get; set; }
        public DateTime DateFinished { get; set; }
        public int RestaurantId { get; set; }
        public int UserId { get; set; }

    }
}
