using Franshy.Entities.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franshy.Entities.ViewModels
{
    public class OrderVm
    {
        public OrderHeader orderheader { get; set; }
        [ValidateNever]
        public IEnumerable<OrderDetail> orderdetails { get; set; }
    }
}
