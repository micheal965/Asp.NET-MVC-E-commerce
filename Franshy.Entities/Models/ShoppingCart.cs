using Franshy.Entities.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Franshy.Entities.ViewModels
{
    public class ShoppingCart
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("product")]
        public int productId { get; set; }
        [ValidateNever]
        public Product product { get; set; }
        public int count { get; set; }
        [ForeignKey("user")]
        public string userId { get; set; }
        [ValidateNever]
        public ApplicationUser user { get; set; }
    }
}
