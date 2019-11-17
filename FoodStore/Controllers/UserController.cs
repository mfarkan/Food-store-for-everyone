using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
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
            await _userManager.SetLockoutEnabledAsync(appUser, true);
            var result = await _userManager.ConfirmEmailAsync(appUser, HttpUtility.UrlDecode(token));
            if (result.Errors.Any())
            {
                result.Errors.ToList().ForEach(e => ModelState.AddModelError(e.Code, e.Description));
            }
            return View();
        }
        [NonAction]
        private string EmailMessage(string userId, string token)
        {
            return $"<a target=\"_blank\" href=\"http://localhost:58889{Url.Action("ActivateUser", "User", new { userId, token = HttpUtility.UrlEncode(token) })}\">Kullanıcınızı doğrulamak için tıklayınız.</a>";
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel user)
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
                    await _messageSender.SendEmailAsync(appUser.Email, "Confirmation Mail For Your Account", this.EmailMessage(appUser.Id.ToString(), token));
                    return Redirect("~/");
                }
            }
            return View();
        }
        public IActionResult Update(string userId)
        {
            return View();
        }
        [HttpPut]
        public IActionResult Update()
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
        public IActionResult SignIn(CreateUserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {

            }
            return View();
        }
    }
}