using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCJustEatClone.Models.ViewModels
{
    public class RegisterConfirmationViewModel
    {
        public User User { get; set; }
        public string ReturnUrl { get; set; }
    }
}
