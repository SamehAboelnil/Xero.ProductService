using System;
using System.ComponentModel.DataAnnotations;

namespace Xero.Product.API.Contracts
{
    public class ProductOption
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
