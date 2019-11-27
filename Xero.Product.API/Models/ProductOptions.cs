using System.Collections.Generic;

namespace Xero.Product.API.Models
{
    public class ProductOptions
    {
        public List<ProductOption> items { get; set; }
        public ProductOptions(List<ProductOption> productOptions)
        {
            items = new List<ProductOption>();
            items.AddRange(productOptions);
        }
    }
}
