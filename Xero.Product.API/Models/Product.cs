using System;
using System.ComponentModel.DataAnnotations;

namespace Xero.Product.API.Models
{
    public class ProductData
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public decimal DeliveryPrice { get; set; }

    }
}
