using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MVCJustEatClone.Models;
using MVCJustEatClone.Models.ViewModels;
using MVCJustEatClone.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MVCJustEatClone.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository userRepository;

        public AccountController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public IActionResult Login(string returnUrl = "/home/index")
        {
            var viewModel = new LoginViewModel()
            {
                ReturnUrl = returnUrl
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var user = await userRepository.GetUserByEmailAndPassword(viewModel.Email, viewModel.Password);

            if (user == null)
            {
                return View(viewModel);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties { IsPersistent = viewModel.RememberMe });

            return LocalRedirect(viewModel.ReturnUrl);
        }

        public IActionResult Register(string returnUrl)
        {
            var viewModel = new RegisterViewModel()
            {
                ReturnUrl = returnUrl
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            if (viewModel.Password != viewModel.PasswordConfirmation)
            {
                return View(viewModel);
            }

            if (await userRepository.CheckUserExistsAsync(viewModel.Email))
            {
                viewModel.AccountExists = true;
                return View(viewModel);
            }

            var user = new User()
            {
                Email = viewModel.Email,
                FirstName = viewModel.FirstName,
                Surname = viewModel.Surname,
                Password = viewModel.Password
            };

            var registeredUser = await userRepository.RegisterUser(user);

            var registerConfirmationViewModel = new RegisterConfirmationViewModel()
            {
                User = registeredUser,
                ReturnUrl = viewModel.ReturnUrl
            };

            return RedirectToAction("RegisterConfirmation", registerConfirmationViewModel);
        }

        public IActionResult RegisterConfirmation(RegisterConfirmationViewModel viewModel)
        {
            return View(viewModel);
        }
    }
}
