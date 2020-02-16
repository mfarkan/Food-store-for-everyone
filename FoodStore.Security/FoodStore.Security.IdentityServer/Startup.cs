using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FoodStore.Domain.UserManagement;
using FoodStore.Security.IdentityServer.AuthorizationRequirements;
using FoodStore.Security.IdentityServer.Data;
using FoodStore.Security.IdentityServer.Describer;
using FoodStore.Security.IdentityServer.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FoodStore.Security.IdentityServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(config =>
            {
                config.UseInMemoryDatabase("Memory");
            });

            services.AddIdentity<ApplicationUser, ApplicationRole>(config =>
            {
                config.Password.RequiredLength = 4;
                config.Password.RequireDigit = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
                config.Password.RequireLowercase = false;

            }) .AddErrorDescriber<CustomErrorDescriber>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "Identity.Cookie";
                config.LoginPath = "/Home/Login";
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

            services.AddHttpContextAccessor();
            services.AddAuthorization(config =>
            {
                config.AddPolicy("Claim.DoB", auth =>
                {
                    auth.AddRequirements(new CustomRequireClaim(ClaimTypes.DateOfBirth));
                });
            });
            services.AddScoped<IAuthorizationHandler, CustomRequireClaimHandler>();
            services.AddControllersWithViews();
        }

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
            // first of all find the page and know the which page will execute.
            app.UseRouting();


            // Who Are you ?
            app.UseAuthentication();

            // Are you allowed ?
            app.UseAuthorization();

            var supportedCultures = new List<CultureInfo> { new CultureInfo("tr-TR"), new CultureInfo("en-US") };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("tr-TR"),
                SupportedUICultures = supportedCultures,
                SupportedCultures = supportedCultures,
                RequestCultureProviders = new[] { new CookieRequestCultureProvider() },
            });

            // if you're ok reach that point.
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
