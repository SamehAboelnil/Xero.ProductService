using System.Collections.Generic;

namespace Xero.Product.API.Models
{
    public class Products
    {
        public List<ProductData> Items { get; }
        public Products(List<ProductData> products)
        {
            Items = new List<ProductData>();
            Items.AddRange(products);
        }
    }
}
