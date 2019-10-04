using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Category
    {
        public long CategoryId { get; set; }

        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        public virtual ICollection<ItemCategory> Items { get; set; }

        public Category()
        {
            Items = new List<ItemCategory>();
        }
    }
}