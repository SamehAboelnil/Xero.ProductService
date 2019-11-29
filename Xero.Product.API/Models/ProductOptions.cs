using System.Collections.Generic;

namespace Xero.Product.API.Models
{
    public class ProductOptions
    {
        public List<ProductOption> Items { get; }
        public ProductOptions(List<ProductOption> productOptions)
        {
            Items = new List<ProductOption>();
            Items.AddRange(productOptions);
        }
    }
}
