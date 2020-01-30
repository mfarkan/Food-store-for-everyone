using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodStore.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class CategoriesController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return await Task.Run(() => View());
        }
        public async Task<IActionResult> Create()
        {
            return await Task.Run(() => View());
        }
        public async Task<IActionResult> Update()
        {
            return await Task.Run(() => View());
        }
        public async Task<IActionResult> Delete()
        {
            return await Task.Run(() => View());
        }
    }
}