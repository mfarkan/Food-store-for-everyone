using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FoodStore.Security.IdentityServer.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using FoodStore.Domain.UserManagement;
using System.Threading.Tasks;

namespace FoodStore.Security.IdentityServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        /// <summary>
        /// it's about the check user allowed to that for some business ? not only check with attribute.
        /// </summary>
        //private readonly IAuthorizationService _authorizationService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public HomeController(
            UserManager<ApplicationUser>userManager,
            IAuthorizationService authorizationService,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            //_authorizationService = authorizationService;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string userName, string passWord)
        {
            // login functionality.
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(user, passWord, false, false);
                if (signInResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Register(string userName, string passWord)
        {
            // register functionality.
            ApplicationUser newUser = new ApplicationUser
            {
                UserName = userName,

            };
            var result = await _userManager.CreateAsync(newUser, passWord);
            if (result.Succeeded)
            {
                // sign user here because if successfully registered user in here simply 
                var signInResult = await _signInManager.PasswordSignInAsync(newUser, passWord, false, false);
                if (signInResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult DoSomething([FromServices] IAuthorizationService _authorizationService)
        {
            // FromServices is locally get what service you want and just use this inside of this method.
            // we do some business here but check if you're allowed to pass here.
            //_authorizationService.AuthorizeAsync(HttpContext.User,)
            return View("Index");
        }
        public IActionResult Register()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
