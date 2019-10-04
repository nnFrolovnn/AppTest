using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Item
    {
        public long ItemId { get; set; }

        public string Name { get; set; }

        public virtual ICollection<ItemCategory> Categories { get; set; }

        public Item()
        {
            Categories = new List<ItemCategory>();
        }
    }
}