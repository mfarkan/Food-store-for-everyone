using FoodStore.Core.Enumarations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Models
{
    public class ApplicationUserViewModel
    {
        [Display(Name = "Id")]
        public Guid Id { get; set; }
        [Display(Name = "UserName")]
        public string UserName { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Gender")]
        public Gender Gender { get; set; }
        [Display(Name = "CreatedAt")]
        public DateTime? CreatedAt { get; set; }
        [Display(Name = "EmailConfirm")]
        public bool EmailConfirmed { get; set; }
        [Display(Name = "PhoneNumberConfirm")]
        public bool PhoneNumberConfirmed { get; set; }
        [Display(Name = "LockOutEnable")]
        public bool LockOutEnabled { get; set; }
        [Display(Name = "AccessFailedCount")]
        public int AccessFailedCount { get; set; }
        public DateTimeOffset? LockOutEnd { get; set; }
    }
}
