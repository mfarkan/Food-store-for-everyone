using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FoodStore.API.Models;
using FoodStore.API.TokenProvider;
using FoodStore.Domain.UserManagement;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FoodStore.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public IConfiguration Configuration { get; }
        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            Configuration = configuration;
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody]LoginModel loginModel)
        {
            string token = string.Empty;
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginModel.UserName, loginModel.PassWord, false, false);
                if (result.Succeeded)
                {
                    var expireMinutes = Configuration["JwtExpireMinutes"];
                    var appUser = await _userManager.FindByNameAsync(loginModel.UserName);
                    token = CustomJwtTokenProvider.GenerateToken(appUser, Configuration["JwtKey"], expireMinutes);
                    TokenModel model = new TokenModel
                    {
                        access_token = token,
                        expires_in = TimeSpan.FromMinutes(Convert.ToDouble(expireMinutes)).TotalSeconds.ToString(),
                        token_type = JwtBearerDefaults.AuthenticationScheme
                    };
                    return new OkObjectResult(model);
                }
            }
            return Unauthorized();
        }
    }
}