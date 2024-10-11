using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Franshy.Entities.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        [ValidateNever]
        [Display(Name = "Photo")]
        public string ImgUrl { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(minimum: 1, maximum: 10000, ErrorMessage = "Price must be between 1 and 10000")]
        public decimal Price { get; set; }
        public decimal InsteadOf { get; set; }
        public bool Isavailable { get; set; } = true;
        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [ValidateNever]
        public Category Category { get; set; }

    }
}
