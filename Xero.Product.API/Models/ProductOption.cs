using System;

namespace Xero.Product.API.Models
{
    public class ProductOption
    {
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
