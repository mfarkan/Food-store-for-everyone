using FoodStore.Core.Enumarations;
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
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public EntityStatusType Status { get; set; }
    }
}
