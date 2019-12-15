using System;
using System.Collections.Generic;

namespace Xero.Product.Domain.Domain
{
    public class ProductData
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal DeliveryPrice { get; set; }

        public List<ProductOption> Options { get; set; }
    }
}
