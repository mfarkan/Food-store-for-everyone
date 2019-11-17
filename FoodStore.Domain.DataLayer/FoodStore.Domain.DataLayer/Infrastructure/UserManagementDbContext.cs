using FoodStore.Domain.UserManagement;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FoodStore.Domain.DataLayer.Infrastructure
{
    public class UserManagementDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid, ApplicationUserClaims, ApplicationUserRole, ApplicationUserLogin, ApplicationRoleClaims, ApplicationUserTokens>
    {
        private Claim claim = new Claim("SuperAdmin", "true");
        public UserManagementDbContext(DbContextOptions<UserManagementDbContext> dbContext) : base(dbContext)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
