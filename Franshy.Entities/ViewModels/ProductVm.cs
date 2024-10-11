using Franshy.Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;


namespace Franshy.Entities.ViewModels
{
    public class ProductVm
    {
        public Product product { get; set; } = new Product();
        [ValidateNever]
        public IEnumerable<Category> categories { get; set; } = new List<Category>();
    }
}
