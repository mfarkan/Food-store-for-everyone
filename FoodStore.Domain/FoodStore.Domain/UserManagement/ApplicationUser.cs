using FoodStore.Core.Enumarations;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Domain.UserManagement
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public Gender Gender { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
