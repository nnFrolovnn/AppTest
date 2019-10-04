using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Category
    {
        public long CategoryId { get; set; }

        public long Name { get; set; }

        public virtual ICollection<ItemCategory> Items { get; set; }

        public Category()
        {
            Items = new List<ItemCategory>();
        }
    }
}