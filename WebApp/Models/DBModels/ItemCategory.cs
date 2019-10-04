using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class ItemCategory
    {
        public long ItemId { get; set; }
        public long CategoryId { get; set; }

        public virtual Item Item { get; set; }
        public virtual Category Category { get; set; }
    }
}