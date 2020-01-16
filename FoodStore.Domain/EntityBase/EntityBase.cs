using FoodStore.Core.Enumarations;
using FoodStore.Domain.UserManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodStore.Domain.EntityBase
{
    public class EntityBase<T>
    {
        public EntityBase()
        {
            CreatedAt = DateTime.Now;
            Status = EntityStatusType.Active;
        }
        public virtual void Activate()
        {
            Status = EntityStatusType.Active;
        }
        public virtual void Passivate()
        {
            Status = EntityStatusType.Passive;
        }
        public virtual void Delete()
        {
            Status = EntityStatusType.Deleted;
        }
        public T Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public ApplicationUser UpdatedBy { get; set; }
        public EntityStatusType Status { get; set; }
    }
}
