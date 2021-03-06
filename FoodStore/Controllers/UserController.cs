﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using FoodStore.Core.Extensions;
using FoodStore.Domain.UserManagement;
using FoodStore.Models;
using FoodStore.Resources;
using FoodStore.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace FoodStore.Controllers
{
    [AutoValidateAntiforgeryToken]
    [AllowAnonymous]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMessageSender _messageSender;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IConfiguration configuration;
        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            IMessageSender messageSender, IStringLocalizer<SharedResource> localizer, IConfiguration config)
        {
            configuration = config;
            _messageSender = messageSender;
            _userManager = userManager;
            _localizer = localizer;
            _signInManager = signInManager;
        }
        #region User
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return RedirectToPage("~/");
            }
            var appUser = await _userManager.FindByIdAsync(userId);
            if (appUser == null)
            {
                ModelState.AddModelError("UserNotFound", _localizer["UserNotFound"]);
                return View();
            }
            var result = await _userManager.ConfirmEmailAsync(appUser, HttpUtility.UrlDecode(token));
            ViewBag.Success = _localizer["DefaultSuccess"];
            if (result.Errors.Any())
            {
                ViewBag.Error = _localizer["DefaultError"];
                ViewBag.Success = string.Empty;
                result.Errors.ToList().ForEach(e => ModelState.AddModelError(e.Code, e.Description));
            }
            return View();
        }
        [NonAction]
        private string EmailMessage(string callBackUrl)
        {
            return $"Please confirm your account by <a href='{(callBackUrl)}'>clicking here</a>.";
        }
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var appUser = await _userManager.FindByEmailAsync(model.Email);
                if (appUser == null)
                {
                    ModelState.AddModelError("UserNotFound", _localizer["UserNotFound"]);
                    return View();
                }
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(appUser);
                var callbackUrl = CallBackUrl("User", "ChangePassword", appUser.Id, HttpUtility.UrlEncode(resetToken), Request.Scheme);
                var isSended = await _messageSender.SendEmailAsync(appUser.Email, "Forgot your password?", this.EmailMessage(callbackUrl));
                if (isSended)
                {
                    ViewBag.Success = _localizer["DefaultSuccess"];
                }
                else
                {
                    ViewBag.Error = _localizer["DefaultError"];
                }
            }
            ModelState.Clear();
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ChangePassword(string userId, string token)
        {
            ChangePasswordViewModel model = new ChangePasswordViewModel
            {
                resetToken = token,
                userId = userId
            };
            return await Task.Run(() => View(model));
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var appUser = await _userManager.FindByIdAsync(model.userId);
                if (appUser == null)
                {
                    ModelState.AddModelError("UserNotFound", _localizer["UserNotFound"]);
                    return View();
                }
                var code = HttpUtility.UrlDecode(model.resetToken).Replace(" ", "+");
                var result = await _userManager.ResetPasswordAsync(appUser, HttpUtility.UrlDecode(code), model.PassWord);
                ViewBag.Success = _localizer["DefaultSuccess"];
                if (result.Errors.Any())
                {
                    ViewBag.Success = string.Empty;
                    ViewBag.Error = _localizer["DefaultError"];
                    result.Errors.ToList().ForEach(e => ModelState.AddModelError(e.Code, e.Description));
                }
            }
            return await Task.Run(() => View(model));
        }
        [NonAction]
        private string CallBackUrl(string controller, string action, Guid userId, string token, string scheme)
        {
            var callbackUrl = Url.Action(
                new Microsoft.AspNetCore.Mvc.Routing.UrlActionContext
                {
                    Action = action,
                    Controller = controller,
                    Protocol = scheme,
                    Values = new { userId, token = HttpUtility.UrlEncode(token) },
                });
            return callbackUrl;
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
                    ViewBag.Error = _localizer["DefaultError"];
                    result.Errors.ToList().ForEach(e => ModelState.AddModelError(e.Code, e.Description));
                }
                else
                {
                    var appUser = await _userManager.FindByNameAsync(applicationUser.UserName);
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
                    var callbackUrl = CallBackUrl("User", "ConfirmEmail", appUser.Id, token, Request.Scheme);
                    var isSended = await _messageSender.SendEmailAsync(appUser.Email, "Confirmation Mail For Your Account", this.EmailMessage(callbackUrl));
                    if (isSended)
                    {
                        ViewBag.Success = _localizer["DefaultSuccess"];
                    }
                    else
                    {
                        ViewBag.Error = _localizer["DefaultError"];
                    }
                }
            }
            ModelState.Clear();
            return View();
        }
        [HttpPut]
        [Authorize]
        public IActionResult Update()
        {
            return View();
        }
        [HttpDelete]
        [Authorize]
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
            return Redirect("~/");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        [AllowAnonymous]
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
                        {
                            return Redirect(ReturnUrl);
                        }
                        return Redirect("~/");
                    }
                    else
                    {
                        await _userManager.AccessFailedAsync(user);
                        var failedCount = await _userManager.GetAccessFailedCountAsync(user);
                        if (failedCount == Convert.ToInt32(configuration.GetCustomerMaxFailedAccessAttempts()))
                        {
                            await _userManager.SetLockoutEndDateAsync(user, new DateTimeOffset(DateTime.Now.AddMinutes(10)));
                            ViewBag.Error = _localizer["UserLocked"];
                        }
                        else
                        {
                            if (result.IsLockedOut)
                            {
                                ViewBag.Error = _localizer["UserLocked"];
                            }
                            else
                            {
                                ViewBag.Error = _localizer["CheckYourLogin"];
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