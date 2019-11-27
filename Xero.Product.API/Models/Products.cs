using System.Collections.Generic;

namespace Xero.Product.API.Models
{
    public class Products
    {
        public List<Product> items { get; set; }
        public Products(List<Product> products)//TODO
        {
            items = new List<Product>();
            items.AddRange(products);
        }
    }
}
