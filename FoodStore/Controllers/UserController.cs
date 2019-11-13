using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Core.ServiceInterfaces;
using FoodStore.Domain.UserManagement;
using FoodStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FoodStore.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMessageSender _messageSender;
        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IMessageSender messageSender)
        {
            _messageSender = messageSender;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        #region User
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> ActivateUser(string userId, string token)
        {
            var appUser = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.ConfirmEmailAsync(appUser, token);
            if (result.Errors.Any())
            {
                result.Errors.ToList().ForEach(e => ModelState.AddModelError(e.Code, e.Description));
            }
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ApplicationUserViewModel user)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = new ApplicationUser()
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    LockoutEnabled = true,
                    EmailConfirmed = false,
                    PhoneNumber = (user.PhonePrefix + user.PhoneNumber).TrimStart('+').Trim(),
                    CreatedAt = DateTime.Now,
                    Gender = user.Sex
                };
                var result = await _userManager.CreateAsync(applicationUser, user.PassWord);
                if (result.Errors.Any())
                {
                    result.Errors.ToList().ForEach(e => ModelState.AddModelError(e.Code, e.Description));
                }
                else
                {
                    var appUser = await _userManager.FindByNameAsync(applicationUser.UserName);
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
                    await _messageSender.SendEmailAsync(token);
                    return Redirect("~/");
                }
            }
            return View();
        }
        public IActionResult Update()
        {
            return View();
        }
        [HttpPut]
        public IActionResult Update(string user)
        {
            return View();
        }
        public IActionResult Delete()
        {
            return View();
        }
        [HttpDelete]
        public IActionResult Delete(string user)
        {
            return View();
        }
        #endregion
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignIn(ApplicationUserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {

            }
            return View();
        }
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignUp(string user)
        {
            return View();
        }
    }
}