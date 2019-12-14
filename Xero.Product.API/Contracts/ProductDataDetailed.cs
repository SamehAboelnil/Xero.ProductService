using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Xero.Product.API.Contracts
{
    public class ProductDataDetailed
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
        public List<ProductOption> Options { get; set; }

    }
}
