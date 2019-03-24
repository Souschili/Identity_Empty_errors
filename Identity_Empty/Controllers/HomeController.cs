using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity_Empty.Models;
using Identity_Empty.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity_Empty.Controllers
{
    
    public class HomeController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public HomeController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            
            return View();
        }

        [HttpGet]  
        [Route("Home/Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("Home/Register")]
        public async Task<IActionResult> Register (RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {

                var user = new ApplicationUser { Email = model.Email, UserName = model.Email };
                var rezult = await _userManager.CreateAsync(user, model.Password);
                if(rezult.Succeeded)
                {
                    // установка куки
                    await _signInManager.SignInAsync(user, false);
                    return new RedirectToActionResult("Index", "Home",null);
                }
                else
                {
                    foreach (var error in rezult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync();
           
            return RedirectToAction("Index", "Home"); ;
        }
    }
}