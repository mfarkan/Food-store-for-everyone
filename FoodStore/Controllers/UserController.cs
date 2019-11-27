using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using FoodStore.Core.Extensions;
using FoodStore.Core.ServiceInterfaces;
using FoodStore.Domain.UserManagement;
using FoodStore.Models;
using FoodStore.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace FoodStore.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMessageSender _messageSender;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IConfiguration configuration;
        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            IMessageSender messageSender, IStringLocalizer<SharedResource> localizer, IConfiguration config, IMapper mapper)
        {
            configuration = config;
            _mapper = mapper;
            _messageSender = messageSender;
            _userManager = userManager;
            _localizer = localizer;
            _signInManager = signInManager;
        }
        #region User
        public IActionResult Index()
        {
            var userList = _userManager.Users.ToList();
            IEnumerable<ApplicationUserViewModel> appUsers = _mapper.Map<List<ApplicationUser>, List<ApplicationUserViewModel>>(userList);
            return View(appUsers);
        }
        public async Task<IActionResult> ActivateUser(string userId, string token)
        {
            var appUser = await _userManager.FindByIdAsync(userId);
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
                ApplicationUser applicationUser = new ApplicationUser
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
        [HttpDelete]
        public async Task<IActionResult> Delete(string userId)
        {
            var appUser = await _userManager.FindByIdAsync(userId);
            var response = await _userManager.DeleteAsync(appUser);
            return new OkObjectResult(response);
        }
        #endregion
        [HttpPost]
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        public IActionResult SignIn(string ReturnUrl)
        {
            @ViewData["returnUrl"] = ReturnUrl;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn([FromForm]LoginUserViewModel userViewModel, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(userViewModel.UserName);
                if (user != null)
                {
                    await _signInManager.SignOutAsync();
                    var result = await _signInManager.PasswordSignInAsync(user, userViewModel.PassWord, userViewModel.Persistent, true);
                    if (result.Succeeded)
                    {
                        await _userManager.ResetAccessFailedCountAsync(user);
                        if (!string.IsNullOrEmpty(ReturnUrl))
                            return Redirect(ReturnUrl);
                        return Redirect("~/");
                    }
                    else
                    {
                        await _userManager.AccessFailedAsync(user);
                        var failedCount = await _userManager.GetAccessFailedCountAsync(user);
                        if (failedCount == Convert.ToInt32(configuration.GetCustomerMaxFailedAccessAttempts()))
                        {
                            await _userManager.SetLockoutEndDateAsync(user, new DateTimeOffset(DateTime.Now.AddMinutes(10)));
                            ModelState.AddModelError("UserLocked", _localizer["UserLocked"]);
                        }
                        else
                        {
                            if (result.IsLockedOut)
                            {
                                ModelState.AddModelError("UserLocked", _localizer["UserLocked"]);
                            }
                            else
                            {
                                ModelState.AddModelError("CheckYourLogin", _localizer["CheckYourLogin"]);
                            }
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("CheckYourLogin", _localizer["CheckYourLogin"]);
                }
            }
            return View();
        }
    }
}