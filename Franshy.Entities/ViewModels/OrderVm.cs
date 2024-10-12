using Franshy.Entities.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Franshy.Entities.ViewModels
{
    public class OrderVm
    {
        public OrderHeader orderheader { get; set; }
        [ValidateNever]
        public IEnumerable<OrderDetail> orderdetails { get; set; }
    }
}
