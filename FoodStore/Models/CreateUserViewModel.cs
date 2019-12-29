using FoodStore.Core.Enumarations;
using FoodStore.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Models
{
    public class CreateUserViewModel
    {

        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        [DataType(DataType.Text)]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        [EmailAddress(ErrorMessage = "InvalidEmailError")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [StringLength(maximumLength: 12, MinimumLength = 6, ErrorMessage = "StringLengthError")]
        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        [Display(Name = "PassWord")]
        public string PassWord { get; set; }

        [DataType(DataType.Password)]
        [StringLength(maximumLength: 12, MinimumLength = 6, ErrorMessage = "StringLengthError")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        [Display(Name = "ComparePassword")]
        [Compare("PassWord", ErrorMessage = "ComparePasswordError")]
        public string ComparePassword { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        [Display(Name = "Gender")]
        public Gender Sex { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        [Display(Name = "PhonePrefix")]
        public string PhonePrefix { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        [StringLength(maximumLength: 15, MinimumLength = 10, ErrorMessage = "StringLengthError")]
        [Display(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }
    }
}
