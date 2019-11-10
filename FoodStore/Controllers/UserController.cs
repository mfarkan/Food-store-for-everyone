﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Domain.UserManagement;
using FoodStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FoodStore.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class UserController : Controller
    {
        readonly UserManager<ApplicationUser> _userManager;
        readonly SignInManager<ApplicationUser> _signInManager;
        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        #region User
        public IActionResult Index()
        {
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
                    CreatedAt = DateTime.Now,
                    Gender = Core.Enumarations.Gender.Male
                };
                var result = await _userManager.CreateAsync(applicationUser, user.PassWord);
                if (result.Errors.Any())
                {
                    result.Errors.ToList().ForEach(e => ModelState.AddModelError(e.Code, e.Description));
                }
                else
                {
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
        public IActionResult SignUp(string user)
        {
            return View();
        }
    }
}