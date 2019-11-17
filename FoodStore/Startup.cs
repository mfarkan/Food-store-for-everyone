using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using FoodStore.Core.Extensions;
using FoodStore.Core.MessageOptions;
using FoodStore.Describer;
using FoodStore.Domain.DataLayer.Infrastructure;
using FoodStore.Domain.UserManagement;
using FoodStore.Resources;
using FoodStore.Validators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
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
            #region Identity
            services.AddIdentity<ApplicationUser, ApplicationRole>(option =>
            {
                option.Lockout = new LockoutOptions
                {
                    MaxFailedAccessAttempts = Convert.ToInt32(Configuration.GetCustomerMaxFailedAccessAttempts()),
                    DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10),
                    AllowedForNewUsers = true,
                };
                option.Password = new PasswordOptions
                {
                    RequireDigit = true,
                    RequiredLength = 6,
                    RequireLowercase = false,
                    RequireNonAlphanumeric = false,
                    RequireUppercase = false,
                };
                option.User = new UserOptions
                {
                    RequireUniqueEmail = true,
                };
            }).AddErrorDescriber<CustomErrorDescriber>().AddDefaultTokenProviders().AddPasswordValidator<CustomPasswordValidator>()
            .AddEntityFrameworkStores<UserManagementDbContext>();
            #endregion
            services.AddNotificationExtensions();
            services.ConfigureApplicationCookie(option =>
            {
                option.LoginPath = new PathString("/User/SignIn");
                option.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                option.Cookie = new CookieBuilder
                {
                    HttpOnly = false,
                    SameSite = SameSiteMode.Lax,
                    SecurePolicy = CookieSecurePolicy.Always,
                    Name = "IdentityCookie",
                };
                option.SlidingExpiration = true;
            });
            services.AddLocalization(o =>
            {
                o.ResourcesPath = "Resources";
            });
            services.AddAntiforgery(option => option.HeaderName = "X-XSRF-Token");
            services.AddControllersWithViews(option =>
            option.Filters.Add(new AutoValidateAntiforgeryTokenAttribute())).AddDataAnnotationsLocalization(o =>
            {
                o.DataAnnotationLocalizerProvider = (type, factory) =>
                {
                    return factory.Create(typeof(SharedResource));
                };
            }).AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix);
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
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
