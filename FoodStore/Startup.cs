using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FoodStore.Domain.DataLayer;
using FoodStore.Domain.DataLayer.Infrastructure;
using FoodStore.Domain.UserManagement;
using FoodStore.Resources;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;

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
        // çeþitli conflarýn yer aldýðý kýsým DI da burda.
        public void ConfigureServices(IServiceCollection services)
        {
            //target assembly ; üzerinde çeþitli operasyonlar yaptýðýn project PMC'de ne seçilirse o aslýnda.
            //Migration assemblye ; Migrationlarýn codelarýnýn bulunduðu uygulama. böylece contextler baþka yerde tanýmlanýp baþka bir projede tutulabilir.
            var migrationAssembly = typeof(UserManagementDbContext).GetTypeInfo().Assembly.GetName().Name;
            services.AddDbContext<UserManagementDbContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("UserDefaultConnection"), sql => sql.MigrationsAssembly(migrationAssembly));
            });
            services.AddIdentity<ApplicationUser, ApplicationRole>(option =>
            {
                option.Lockout = new LockoutOptions
                {
                    DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10),
                };
                option.Password = new PasswordOptions
                {
                    RequireDigit = true,
                    RequiredLength = 12,
                };
                option.User = new UserOptions
                {
                    RequireUniqueEmail = true,
                };
            }).AddEntityFrameworkStores<UserManagementDbContext>();
            services.AddLocalization(o =>
            {
                o.ResourcesPath = "Resources";
            });
            services.AddControllersWithViews()
                .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix);
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
            var supportedCultures = new List<CultureInfo> { new CultureInfo("tr-TR"), new CultureInfo("en-US") };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("tr-TR"),
                SupportedUICultures = supportedCultures,
                SupportedCultures = supportedCultures,
                RequestCultureProviders = new[] { new CookieRequestCultureProvider() },
            });

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
