using FoodStore.Domain.CategoryManagement;
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
        public DbSet<Category> Categories { get; set; }
        public UserManagementDbContext(DbContextOptions<UserManagementDbContext> dbContext) : base(dbContext)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("public");
            builder.Entity<Category>(table =>
            {
                table.ToTable("Category", "public");
                table.HasComment("Where categories in kept :)");
                table.HasKey(key => key.Id);
                table.Property(p => p.CategoryDescription).HasMaxLength(200).HasComment("This column is a category's explanation");
                table.HasOne(q => q.UpdatedBy);
                table.HasOne(q => q.CreatedBy);
            });
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("public");
        }
    }
}
