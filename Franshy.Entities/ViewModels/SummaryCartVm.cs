using Franshy.Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;


namespace Franshy.Entities.ViewModels
{
    public class SummaryCartVm
    {
        public OrderHeader orderHeader { get; set; }
        [ValidateNever]
        public IEnumerable<ShoppingCart> shoppingcarts { get; set; }
    }
}
