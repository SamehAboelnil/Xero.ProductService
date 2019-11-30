using System;

namespace Xero.Product.Domain.Models
{
    public class ProductOption
    {
        public Guid ProductId { get; set; }
        public ProductData Product { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
