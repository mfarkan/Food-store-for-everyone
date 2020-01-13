using FoodStore.Domain.EntityBase;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodStore.Domain.CategoryManagement
{
    public class Category : EntityBase<Guid>
    {
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public int CategoryPriority { get; set; }
    }
}
