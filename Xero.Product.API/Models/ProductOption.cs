using System;
using System.ComponentModel.DataAnnotations;

namespace Xero.Product.API.Models
{
    public class ProductOption
    {
        [Required]
        public Guid ProductId { get; set; }
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
