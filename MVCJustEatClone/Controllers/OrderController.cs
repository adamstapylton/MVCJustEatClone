using Microsoft.AspNetCore.Mvc;
using MVCJustEatClone.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCJustEatClone.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Checkout()
        {
            var checkoutViewModel = new CheckoutViewModel();

            return View(checkoutViewModel);
        }
    }
}
