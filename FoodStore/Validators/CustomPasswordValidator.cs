using FoodStore.Domain.UserManagement;
using FoodStore.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodStore.Validators
{
    public class CustomPasswordValidator : IPasswordValidator<ApplicationUser>
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        public CustomPasswordValidator(IStringLocalizer<SharedResource> localizer)
        {
            _localizer = localizer;
        }
        public Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user, string password)
        {
            List<IdentityError> errors = new List<IdentityError>();

            if (password.ToLowerInvariant().Contains(user.UserName.ToLowerInvariant())) //Password içerisinde username kontrolü
            {
                errors.Add(new IdentityError { Code = "PasswordContainsUserName", Description = _localizer["PasswordContainsUserName"] });
            }
            if (!errors.Any())
            {
                return Task.FromResult(IdentityResult.Success);
            }
            else
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }
        }
    }
}
