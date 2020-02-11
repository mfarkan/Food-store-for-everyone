using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FoodStore.Security.IdentityServer.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;

namespace FoodStore.Security.IdentityServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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
        public IActionResult Authenticate()
        {
            var kubraClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,"Kubra"),
                new Claim(ClaimTypes.Email,"arkankubra@yandex.com.tr"),
                new Claim("Kubra.Says","My husbanddd")
            };

            var licenceClaims = new List<Claim>()
            {
                new Claim("Driving.License","B"),
                new Claim(ClaimTypes.Name,"Kubra arkan")
            };
            var licenceIdentity = new ClaimsIdentity(licenceClaims, "Goverment");
            var kubraIdentity = new ClaimsIdentity(kubraClaims, "Kubras Identity");

            var userIdentity = new ClaimsPrincipal(new[] { kubraIdentity, licenceIdentity });

            HttpContext.SignInAsync(userIdentity);

            return RedirectToAction("Index");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
