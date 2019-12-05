using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Authorization.CustomAuthorization
{
    public class CustomAuthFilter : IAsyncAuthorizationFilter
    {
        public CustomAuthFilter(IAuthorizationService service, CustomAuthRequirement requirement)
        {
            this._service = service;
            this._requirement = requirement;
        }
        private readonly IAuthorizationService _service;
        private readonly CustomAuthRequirement _requirement;
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var isAuth = await _service.AuthorizeAsync(context.HttpContext.User, context.ActionDescriptor.ToString(), _requirement);
            if (!isAuth.Succeeded)
            {
                context.Result = new ChallengeResult();
            }
        }
    }
}
