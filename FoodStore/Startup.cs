using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FoodStore.Domain.DataLayer;
using FoodStore.Domain.DataLayer.Infrastructure;
using FoodStore.Domain.UserManagement;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FoodStore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //target assembly ; üzerinde çeþitli operasyonlar yaptýðýn project PMC'de ne seçilirse o aslýnda.
            //Migration assemblye ; Migrationlarýn codelarýnýn bulunduðu uygulama. böylece contextler baþka yerde tanýmlanýp baþka bir projede tutulabilir.
            var migrationAssembly = typeof(UserManagementDbContext).GetTypeInfo().Assembly.GetName().Name;
            services.AddDbContext<UserManagementDbContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("UserDefaultConnection"), sql => sql.MigrationsAssembly(migrationAssembly));
            });
            services.AddIdentity<ApplicationUser, ApplicationRole>().AddEntityFrameworkStores<UserManagementDbContext>();
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
