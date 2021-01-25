using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVCJustEatClone.Models.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Restaurant> Restaurants { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter search details")]
        public string Search { get; set; }
        public bool SearchUsed { get; set; }
    }
}
