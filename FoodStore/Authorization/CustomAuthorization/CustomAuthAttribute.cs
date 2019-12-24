//using FoodStore.Core.Enumarations;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace FoodStore.Authorization.CustomAuthorization
//{
//    public class CustomAuthAttribute : TypeFilterAttribute
//    {
//        public Permissions[] permissions { get; set; }
//        public CustomAuthAttribute(params Permissions[] permissions) : base(typeof(CustomAuthFilter))
//        {
//            this.permissions = permissions;
//            Arguments = new[] { new CustomAuthRequirement(this.permissions) };
//        }
//    }
//}
