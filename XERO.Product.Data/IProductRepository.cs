using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xero.Product.Data
{
    public interface IProductRepository
    {
        Task<Product> AddProduct(Product product);

        Task<ProductOption> AddProductOption(ProductOption productOption);
        Task<Product> DeleteProduct(Guid id);

        Task<ProductOption> DeleteProductOption(Guid productId, Guid productOptionId);
        Task<IEnumerable<Product>> GetAllProducts(string name);
        Task<Product> GetProduct(Guid id);
        Task<IEnumerable<ProductOption>> GetOptions(Guid productId);

        Task<IEnumerable<ProductOption>> GetOptionById(Guid productId, Guid OptionId);
        Task<Product> UpdateProduct(Guid id, Product product);

        Task<ProductOption> UpdateProductOption(Guid id, Guid option, ProductOption product);
    }
}
