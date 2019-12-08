using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xero.Product.Data
{
    public interface IProductRepository
    {
        Task<ProductData> AddProduct(ProductData product);

        Task<ProductOption> AddProductOption(Guid productId, ProductOption productOption);
        Task<ProductData> DeleteProduct(Guid id);

        Task<ProductOption> DeleteProductOption(Guid productId, Guid productOptionId);
        Task<IEnumerable<ProductData>> GetAllProducts(string name);
        Task<ProductData> GetProduct(Guid id);
        Task<IEnumerable<ProductOption>> GetOptions(Guid productId);

        Task<ProductOption> GetOptionById(Guid productId, Guid OptionId);
        Task<ProductData> UpdateProduct(Guid id, ProductData product);

        Task<bool> IsProductExist(Guid OptionId);
        Task<bool> IsProductOptionExist(Guid ProductId, Guid OptionId);

        Task<ProductOption> UpdateProductOption(Guid id, Guid productOption, ProductOption product);
    }
}
