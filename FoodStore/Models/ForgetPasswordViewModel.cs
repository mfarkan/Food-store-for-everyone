using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Models
{
    public class ForgetPasswordViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredError")]
        [EmailAddress(ErrorMessage = "InvalidEmailError")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
