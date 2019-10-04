using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApp.Helpers;

namespace WebApp.Models
{
    public class ItemViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EnsureOneElement(ErrorMessage = "At least one category is required")]
        [EnsureUniqueElements(ErrorMessage = "categories must be unique")]
        public List<long> Categories { get; set; }
    }

    public class ItemViewModelApi
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EnsureOneElement(ErrorMessage = "At least one category is required")]
        [EnsureUniqueElements(ErrorMessage = "categories must be unique")]
        public List<CategoryViewModel> Categories { get; set; }
    }

    public class CategoryViewModel
    {
        public long CategoryId { get; set; }

        [Required]
        [MinLength(3)]
        public string Name { get; set; }
    }

}