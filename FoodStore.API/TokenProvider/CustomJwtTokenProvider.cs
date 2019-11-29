using FoodStore.Domain.UserManagement;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FoodStore.API.TokenProvider
{
    public class CustomJwtTokenProvider
    {
        public static string GenerateToken(ApplicationUser user, string jwtKey, string jwtExpireMinutes)
        {
            var claim = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };
            var keyByteArray = Encoding.ASCII.GetBytes(jwtKey);
            var signInKey = new SymmetricSecurityKey(keyByteArray);
            var expiresMinutes = DateTime.Now.AddMinutes(Convert.ToDouble(jwtExpireMinutes));
            var token = new JwtSecurityToken(claims: claim, expires: expiresMinutes, signingCredentials: new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
