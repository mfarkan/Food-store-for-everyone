using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Domain.UserManagement
{
    public class ApplicationRole : IdentityRole<Guid>
    {
    }
}
