using FoodStore.Domain.UserManagement;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Domain.DataLayer.Infrastructure
{
    public class UserManagementDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public UserManagementDbContext(DbContextOptions<UserManagementDbContext> dbContext) : base(dbContext)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
