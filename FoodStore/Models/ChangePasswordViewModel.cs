﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Models
{
    public class ChangePasswordViewModel
    {
        public string userId { get; set; }
        public string resetToken { get; set; }

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
    }
}
