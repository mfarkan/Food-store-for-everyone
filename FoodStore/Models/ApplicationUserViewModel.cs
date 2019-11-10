﻿using FoodStore.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Models
{
    public class ApplicationUserViewModel
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
        public string ComparePassword { get; set; }
    }
}
