using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Microsoft.AspNetCore.Identity;
using FoodStore.Services.ServiceInterfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using FoodStore.Resources;
using FoodStore.Domain.UserManagement;
using FoodStore.Controllers;
using FoodStore.Tests.TestClasses;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FoodStore.Tests
{
    [TestFixture]
    public class UserControllerTests
    {
        public UserControllerTests()
        {
        }
        [Test]
        public void One_Equal_One_ReturnTrue()
        {
            Assert.AreEqual(1, 1);
        }
        [Test]
        public async Task Create_User_IdentityResult_Success()
        {
            var controller = IdentityTests.GetUserController();
            var fakeUser = IdentityTests.GetCreateUserViewModel();
            var result = await controller.Create(fakeUser);

            Assert.IsAssignableFrom<ViewResult>(result);
            Assert.IsNotNull(result, "UserController_Create viewResult değeri boş geldi");
        }
        [Test]
        public async Task Delete_User_IdentityResult_Success()
        {
            var controller = IdentityTests.GetUserController();
            var fakeUser = IdentityTests.GetUser();
            var result = await controller.Delete(fakeUser.Id.ToString()) as OkObjectResult;

            Assert.AreEqual(result.StatusCode, 200);
            Assert.IsNotNull(result);
        }
        [Test]
        public async Task Confirm_Email_Return_Main_Page()
        {
            var controller = IdentityTests.GetUserController();
            var result = await controller.ConfirmEmail(string.Empty, string.Empty);
            Assert.IsAssignableFrom<RedirectToPageResult>(result);
            var redirectPageResult = result as RedirectToPageResult;
            Assert.AreEqual("~/", redirectPageResult.PageName);
        }
        [Test]
        public async Task Confirm_Email_Return_ModelState_Error()
        {
            var controller = IdentityTests.GetUserController();
            var result = await controller.ConfirmEmail(new Guid().ToString(), "generatedToken");
            Assert.IsAssignableFrom<ViewResult>(result);

            var viewResult = result as ViewResult;
            Assert.IsTrue(controller.ViewData.ModelState.ErrorCount > 0);
            Assert.AreEqual(controller.ViewData.ModelState["UserNotFound"].ValidationState, ModelValidationState.Invalid);
            Assert.IsTrue(!viewResult.ViewData.ModelState.IsValid);
        }
    }
}
