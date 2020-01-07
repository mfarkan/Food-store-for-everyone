using FoodStore.Core.MessageOptions;
using FoodStore.Domain.UserManagement;
using FoodStore.Models;
using FoodStore.Resources;
using FoodStore.Services.ServiceInterfaces;
using FoodStore.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GenFu;
using FoodStore.Controllers;

namespace FoodStore.Tests.TestClasses
{
    public class IdentityTests
    {
        public static UserController GetUserController()
        {
            var _userManager = GetMockUserManager().Object;
            var _signInManager = GetMockSignInManager().Object;
            var _messageSender = GetMessageSender().Object;
            var _localizer = GetLocalization();
            var configuration = GetConfiguration().Object;
            var controller = new UserController(_userManager, _signInManager, _messageSender, _localizer, configuration);

            controller.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.Url = GetUrlHelper().Object;
            controller.ControllerContext.HttpContext.Request.Headers["food-store"] = "123456";
            return controller;
        }
        public static CreateUserViewModel GetCreateUserViewModel()
        {
            GenFu.GenFu.Configure<CreateUserViewModel>()
                .Fill(q => q.PhoneNumber)
                .Fill(q => q.UserName)
                .Fill(q => q.Email, q => { return string.Format("{0}.{0}@gmail.com", q.UserName, q.PhoneNumber); })
                .Fill(q => q.PhonePrefix, "90")
                .Fill(q => q.Sex, Core.Enumarations.Gender.Female);
            return A.New<CreateUserViewModel>();
        }
        public static ApplicationUser GetUser()
        {
            A.Configure<ApplicationUser>()
                .Fill(q => q.Email, q => { return string.Format("{0}.{1}@gmail.com", q.UserName, q.PhoneNumber); })
                .Fill(q => q.PhoneNumber, "905384811896")
                .Fill(q => q.AccessFailedCount).WithinRange(0, 3)
                .Fill(q => q.CreatedAt, DateTime.Now)
                .Fill(q => q.Gender, Core.Enumarations.Gender.Female)
                .Fill(q => q.EmailConfirmed, true)
                .Fill(q => q.PhoneNumberConfirmed, true)
                .Fill(q => q.Id, new Guid("FFC42A97-C75D-4F8B-85D7-9044BE829755"));
            return A.New<ApplicationUser>();
        }
        public static Mock<Microsoft.AspNetCore.Mvc.IUrlHelper> GetUrlHelper()
        {
            var mockUrlHelper = new Mock<Microsoft.AspNetCore.Mvc.IUrlHelper>(MockBehavior.Strict);
            mockUrlHelper
                .Setup(
                    x => x.Action(
                        It.IsAny<Microsoft.AspNetCore.Mvc.Routing.UrlActionContext>()
                    )
                )
                .Returns("callbackUrl");
            return mockUrlHelper;
        }
        public static Mock<UserManager<ApplicationUser>> GetMockUserManager()
        {
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var manager = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            manager.Object.UserValidators.Add(new UserValidator<ApplicationUser>());
            manager.Object.PasswordValidators.Add(new PasswordValidator<ApplicationUser>());

            manager.Setup(q => q.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            manager.Setup(q => q.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(GetUser());
            manager.Setup(q => q.DeleteAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);
            manager.Setup(q => q.FindByIdAsync(It.Is<string>(a => !Equals(a, "FFC42A97-C75D-4F8B-85D7-9044BE829755")))).ReturnsAsync(default(ApplicationUser));
            manager.Setup(q => q.FindByIdAsync(It.Is<string>(a => Equals(a, "FFC42A97-C75D-4F8B-85D7-9044BE829755")))).ReturnsAsync(GetUser());
            manager.Setup(q => q.GenerateEmailConfirmationTokenAsync(It.IsAny<ApplicationUser>())).ReturnsAsync("generatedToken");
            manager.Setup(q => q.ConfirmEmailAsync(It.IsAny<ApplicationUser>(), It.Is<string>(s => Equals(s, "generatedToken")))).ReturnsAsync(IdentityResult.Success);
            manager.Setup(q => q.ConfirmEmailAsync(It.IsAny<ApplicationUser>(), It.Is<string>(s => !Equals(s, "generatedToken"))))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Code = "InvalidToken", Description = "Invalid token." }));
            manager.Setup(q => q.ResetAccessFailedCountAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);
            manager.Setup(q => q.GetAccessFailedCountAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(1);
            manager.Setup(q => q.SetLockoutEndDateAsync(It.IsAny<ApplicationUser>(), It.IsAny<DateTimeOffset>())).ReturnsAsync(IdentityResult.Success);
            return manager;
        }
        public static Mock<SignInManager<ApplicationUser>> GetMockSignInManager()
        {
            var _contextAccessor = new Mock<IHttpContextAccessor>();
            var userManager = GetMockUserManager();
            var _userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
            Mock<SignInManager<ApplicationUser>> mockApiSignInManager = new Mock<SignInManager<ApplicationUser>>(userManager.Object,
                           _contextAccessor.Object, _userPrincipalFactory.Object, null, null, null, null);

            mockApiSignInManager.Setup(q => q.PasswordSignInAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(SignInResult.Success);
            mockApiSignInManager.Setup(q => q.SignOutAsync()).Returns(Task.CompletedTask);
            mockApiSignInManager.Setup(q => q.IsSignedIn(It.IsAny<ClaimsPrincipal>())).Returns(true);

            return mockApiSignInManager;
        }
        public static Mock<IMessageSender> GetMessageSender()
        {
            IOptions<MailSettings> settings = Options.Create(It.IsAny<MailSettings>());
            var messageManager = new Mock<IMessageSender>();
            messageManager.Setup(q => q.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);
            messageManager.Setup(q => q.SendSmsAsync()).Returns(Task.CompletedTask);
            return messageManager;
        }
        public static Mock<IConfiguration> GetConfiguration()
        {
            var configManager = new Mock<IConfiguration>();
            configManager.Setup(q => q["DefaultSuccess"]).Returns("Başarıyla Kaydedildi.");
            configManager.Setup(q => q["DefaultError"]).Returns("İşlem başarısız.");
            configManager.Setup(q => q["CookieName"]).Returns("ff483d1ff591898a9942916050d2ca3f");
            return configManager;
        }
        public static IStringLocalizer<SharedResource> GetLocalization()
        {
            var options = Options.Create(new LocalizationOptions { ResourcesPath = "Resources" });
            var factory = new ResourceManagerStringLocalizerFactory(options, NullLoggerFactory.Instance);
            var localizer = new StringLocalizer<SharedResource>(factory);
            return localizer;
        }
    }
}
