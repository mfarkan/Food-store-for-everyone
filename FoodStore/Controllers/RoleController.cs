using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Domain.UserManagement;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FoodStore.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class RoleController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        public RoleController(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }
        #region Role
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(string role)
        {
            return View();
        }
        public IActionResult Update()
        {
            return View();
        }
        [HttpPut]
        public IActionResult Update(string role)
        {
            return View();
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string roleId)
        {
            var appRole = await _roleManager.FindByIdAsync(roleId);
            if (appRole == null)
            {
                return NotFound();
            }
            var response = await _roleManager.DeleteAsync(appRole);
            return new OkObjectResult(response);
        }
        #endregion
    }
}