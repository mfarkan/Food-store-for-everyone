using FoodStore.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Describer
{
    public class CustomErrorDescriber : IdentityErrorDescriber
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        public CustomErrorDescriber(IStringLocalizer<SharedResource> localizer)
        {
            _localizer = localizer;
        }
        public override IdentityError DuplicateUserName(string userName) => new IdentityError() { Code = "DuplicateUserName", Description = _localizer["DuplicateUserName"] };
        public override IdentityError DuplicateEmail(string email) => new IdentityError() { Code = "DuplicateEmail", Description = _localizer["DuplicateEmail"] };
        public override IdentityError DuplicateRoleName(string role) => new IdentityError() { Description = _localizer["DuplicateRoleName"], Code = "DuplicateRoleName" };
        public override IdentityError InvalidEmail(string email) => new IdentityError() { Code = "InvalidEmail", Description = _localizer["InvalidEmail"] };
        public override IdentityError InvalidUserName(string userName) => new IdentityError { Code = "InvalidUserName", Description = _localizer["InvalidUserName"] };
        public override IdentityError InvalidRoleName(string role) => new IdentityError { Code = "InvalidRoleName", Description = _localizer["InvalidRoleName"] };
        public override IdentityError InvalidToken() => new IdentityError { Code = "InvalidToken", Description = _localizer["InvalidToken"] };
        public override IdentityError PasswordMismatch() => new IdentityError { Code = "PasswordMismatch", Description = _localizer["PasswordMismatch"] };
        public override IdentityError UserAlreadyHasPassword() => new IdentityError { Code = "UserAlreadyHasPassword", Description = _localizer["UserAlreadyHasPassword"] };
        public override IdentityError UserAlreadyInRole(string role) => new IdentityError { Code = "UserAlreadyInRole", Description = _localizer["UserAlreadyInRole"] };
        public override IdentityError UserNotInRole(string role) => new IdentityError { Code = "UserNotInRole", Description = _localizer["UserNotInRole"] };
        public override IdentityError PasswordTooShort(int length) => new IdentityError { Code = "PasswordTooShort", Description = _localizer["PasswordTooShort"] };
        public override IdentityError UserLockoutNotEnabled() => new IdentityError { Code = "UserLockoutNotEnabled", Description = _localizer["UserLockoutNotEnabled"] };
        public override IdentityError ConcurrencyFailure() => new IdentityError { Code = "ConcurrencyFailure", Description = _localizer["ConcurrencyFailure"] };
        public override IdentityError LoginAlreadyAssociated() => new IdentityError { Code = "LoginAlreadyAssociated", Description = _localizer["LoginAlreadyAssociated"] };
        public override IdentityError RecoveryCodeRedemptionFailed() => new IdentityError { Code = "RecoveryCodeRedemptionFailed", Description = _localizer["RecoveryCodeRedemptionFailed"] };
        public override IdentityError DefaultError() => new IdentityError { Code = "DefaultError", Description = _localizer["DefaultError"] };
        public override IdentityError PasswordRequiresDigit() => new IdentityError { Code = "PasswordRequiresDigit", Description = _localizer["PasswordRequiresDigit"] };
        public override IdentityError PasswordRequiresLower() => new IdentityError { Code = "PasswordRequiresLower", Description = _localizer["PasswordRequiresLower"] };
        public override IdentityError PasswordRequiresNonAlphanumeric() => new IdentityError { Code = "PasswordRequiresNonAlphanumeric", Description = _localizer["PasswordRequiresNonAlphanumeric"] };
        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars) => new IdentityError { Code = "PasswordRequiresUniqueChars", Description = _localizer["PasswordRequiresUniqueChars"] };
        public override IdentityError PasswordRequiresUpper() => new IdentityError { Code = "PasswordRequiresUpper", Description = _localizer["PasswordRequiresUpper"] };
    }
}
