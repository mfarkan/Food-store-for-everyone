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
        [Test]
        public async Task Confirm_Email_Return_ModelState_Error_InvalidToken()
        {
            var controller = IdentityTests.GetUserController();
            var result = await controller.ConfirmEmail("FFC42A97-C75D-4F8B-85D7-9044BE829755", "fakeToken");

            Assert.IsAssignableFrom<ViewResult>(result);

            var viewResult = result as ViewResult;
            Assert.IsTrue(controller.ViewData.ModelState.ErrorCount > 0);
            Assert.AreEqual(controller.ViewData.ModelState["InvalidToken"].ValidationState, ModelValidationState.Invalid);
            Assert.IsTrue(!viewResult.ViewData.ModelState.IsValid);
        }
        [Test]
        public async Task Forgot_Password_ModelState_Invalid()
        {
            var controller = IdentityTests.GetUserController();
            controller.ModelState.AddModelError("InvalidToken", "Invalid token.");
            var result = await controller.ForgotPassword(IdentityTests.GetForgetPasswordViewModel(string.Empty));

            Assert.IsAssignableFrom<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.IsTrue(controller.ViewData.ModelState.ErrorCount == 0);
            Assert.IsTrue(viewResult.ViewData.ModelState.IsValid);
        }
        [Test]
        public async Task Forgot_Password_UserNotFound()
        {
            var controller = IdentityTests.GetUserController();
            var result = await controller.ForgotPassword(IdentityTests.GetForgetPasswordViewModel(string.Empty));

            Assert.IsAssignableFrom<ViewResult>(result);

            var viewResult = result as ViewResult;
            Assert.IsTrue(controller.ViewData.ModelState.ErrorCount > 0);
            Assert.AreEqual(controller.ViewData.ModelState["UserNotFound"].ValidationState, ModelValidationState.Invalid);
            Assert.IsFalse(viewResult.ViewData.ModelState.IsValid);
        }
        [Test]
        public async Task Forgot_Password_Email_Sended()
        {
            var controller = IdentityTests.GetUserController();
            var result = await controller.ForgotPassword(IdentityTests.GetForgetPasswordViewModel("foodstore@gmail.com"));
            Assert.IsAssignableFrom<ViewResult>(result);
            var successMessage = controller.ViewBag.Success;
            Assert.IsNotNull(successMessage);
            Assert.IsAssignableFrom<LocalizedString>(successMessage);
            var localizedString = successMessage as LocalizedString;
            Assert.IsFalse(localizedString.ResourceNotFound);
            Assert.AreEqual(localizedString.Value, "Operation successfuly completed.");

        }
        [Test]
        public async Task Change_Password_ModelState_Invalid()
        {
            var controller = IdentityTests.GetUserController();
            controller.ModelState.AddModelError("InvalidToken", "Invalid token.");
            var result = await controller.ChangePassword(IdentityTests.GetChangePasswordViewModel("FFC42A97-C75D-4F8B-85D7-9044BE829755", "password1", "generatedToken"));

            Assert.IsAssignableFrom<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.IsTrue(controller.ViewData.ModelState.ErrorCount == 1);
            Assert.IsFalse(viewResult.ViewData.ModelState.IsValid);
        }
        [Test]
        public async Task Change_Password_User_NotFound()
        {
            var controller = IdentityTests.GetUserController();
            var result = await controller.ChangePassword(IdentityTests.GetChangePasswordViewModel(string.Empty, "password1", "generatedToken"));

            Assert.IsAssignableFrom<ViewResult>(result);

            var viewResult = result as ViewResult;
            Assert.IsTrue(controller.ViewData.ModelState.ErrorCount > 0);
            Assert.AreEqual(controller.ViewData.ModelState["UserNotFound"].ValidationState, ModelValidationState.Invalid);
            Assert.IsFalse(viewResult.ViewData.ModelState.IsValid);
        }
        [Test]
        public async Task Change_Password_Invalid_Token()
        {
            var controller = IdentityTests.GetUserController();
            var result = await controller.ChangePassword(IdentityTests.GetChangePasswordViewModel("FFC42A97-C75D-4F8B-85D7-9044BE829755", "password1", "failToken"));

            Assert.IsAssignableFrom<ViewResult>(result);

            var viewResult = result as ViewResult;
            Assert.IsTrue(controller.ViewData.ModelState.ErrorCount > 0);
            Assert.AreEqual(controller.ViewData.ModelState["InvalidToken"].ValidationState, ModelValidationState.Invalid);
            Assert.IsFalse(viewResult.ViewData.ModelState.IsValid);
        }
        [Test]
        public async Task Change_Password_Identity_Result_Success()
        {
            var controller = IdentityTests.GetUserController();
            var result = await controller.ChangePassword(IdentityTests.GetChangePasswordViewModel("FFC42A97-C75D-4F8B-85D7-9044BE829755", "password1", "generatedToken"));

            Assert.IsAssignableFrom<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.IsTrue(controller.ViewData.ModelState.ErrorCount == 0);
            Assert.IsTrue(viewResult.ViewData.ModelState.IsValid);
        }
        [Test]
        public void GetUserTests()
        {
            var user = IdentityTests.GetUser();
            Assert.IsNotNull(user);
        }
        [Test]
        public async Task Sign_In_Model_State_Invalid()
        {
            var controller = IdentityTests.GetUserController();
            controller.ModelState.AddModelError("InvalidToken", "Invalid token.");
            var result = await controller.SignIn(IdentityTests.GetLoginUserViewModel("mfarkan"), string.Empty);
            Assert.IsAssignableFrom<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.IsTrue(controller.ViewData.ModelState.ErrorCount == 1);
            Assert.IsFalse(viewResult.ViewData.ModelState.IsValid);
        }
        [Test]
        public async Task Sign_In_User_Not_Found()
        {
            var controller = IdentityTests.GetUserController();
            var result = await controller.SignIn(IdentityTests.GetLoginUserViewModel("fakeUser"), string.Empty);
            Assert.IsAssignableFrom<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.IsTrue(controller.ViewData.ModelState.ErrorCount > 0);
            Assert.AreEqual(controller.ViewData.ModelState["CheckYourLogin"].ValidationState, ModelValidationState.Invalid);
            Assert.IsFalse(viewResult.ViewData.ModelState.IsValid);
        }
        public async Task Sign_In_Access_Fail_Count_Return_1()
        {

        }
    }
}
