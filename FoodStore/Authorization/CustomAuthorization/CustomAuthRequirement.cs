using FoodStore.Core.Enumarations;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Authorization.CustomAuthorization
{
    public class CustomAuthRequirement : IAuthorizationRequirement
    {
        public Permissions[] permissions;
        public CustomAuthRequirement(params Permissions[] permissions)
        {
            this.permissions = permissions;
        }
    }
}
