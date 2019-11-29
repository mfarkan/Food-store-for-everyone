using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FoodStore.Core.Enumarations
{
    public enum Gender
    {
        [Display(Name = "Female", ShortName = "FML")]
        Female = 1,
        [Display(Name = "Male", ShortName = "ML")]
        Male = 2,
    }
}
