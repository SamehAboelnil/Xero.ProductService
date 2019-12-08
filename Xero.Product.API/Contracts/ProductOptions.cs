using System.Collections.Generic;

namespace Xero.Product.API.Contracts
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
