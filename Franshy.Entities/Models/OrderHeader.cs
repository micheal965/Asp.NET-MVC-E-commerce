using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Franshy.Entities.Models
{
    public class OrderHeader
    {
        public int Id { get; set; }

        public string ApplicationUserId { get; set; }
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
        [Required(ErrorMessage = "Name is Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Address is Required")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Phone is Required")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [Required(ErrorMessage = "City is Required")]
        public string City { get; set; }

        public DateTime OrderDate { get; set; }
        public DateTime ShippingDate { get; set; }

        public decimal TotalPrice { get; set; }
        public string? OrderStatus { get; set; }
        public string? PaymentStatus { get; set; }
        public string? TrackingNumber { get; set; }
        public DateTime PaymentDate { get; set; }

        //Stripe Properties
        public string? SessionId { get; set; }
        public string? PaymentIntentId { get; set; }


    }
}
