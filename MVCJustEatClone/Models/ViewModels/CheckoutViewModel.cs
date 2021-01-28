using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVCJustEatClone.Models.ViewModels
{
    public class CheckoutViewModel
    {
        public Order Order { get; set; }
        public Restaurant Restaurant { get; set; }
        [Required]
        [MinLength(1), MaxLength(10)]
        public string Title { get; set; }
        [Required]
        [MinLength(1), MaxLength(20)]
        public string FirstName { get; set; }
        [Required]
        [MinLength(1), MaxLength(30)]
        public string Surname { get; set; }


    }
}
