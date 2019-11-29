using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodStore.Domain.UserManagement
{
    public class ApplicationUserClaims : IdentityUserClaim<Guid>
    {
    }
}
