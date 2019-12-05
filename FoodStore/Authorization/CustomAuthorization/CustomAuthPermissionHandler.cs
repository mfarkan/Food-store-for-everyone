using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FoodStore.Authorization.CustomAuthorization
{
    public class CustomAuthPermissionHandler : AuthorizationHandler<CustomAuthRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomAuthRequirement requirement)
        {
            if (context == null || context.User == null)
            {
                context.Fail();
            }
            var userId = context.User.FindFirst(c => string.CompareOrdinal(c.Type, ClaimTypes.NameIdentifier) == 0);
            if (userId == null)
            {
                context.Fail();
            }
            else
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
