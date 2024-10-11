using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Franshy.Entities.Models
{
    public class Category
    {
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is Required")]
        public string Name { get; set; }
        [ValidateNever]
        public IEnumerable<Product> Products { get; set; }
    }
}
