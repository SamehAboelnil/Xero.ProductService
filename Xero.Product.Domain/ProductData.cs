using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xero.Product.Data;

namespace Xero.Product.Domain
{
    public class ProductData
    {
        private readonly IProductRepository productRepository;
        public ProductData(IProductRepository productContext)
        {
            productRepository = productContext;
        }

        public async Task<IEnumerable<Models.Product>> GetAllProducts(string name)
        {
            IEnumerable<Data.Product> products = await productRepository.GetAllProducts(name);
            return products.Select(x => Mapping.Mapper.Map<Models.Product>(x)).ToList();

        }

        public async Task<Models.Product> GetProduct(Guid id)
        {
            Data.Product product = await productRepository.GetProduct(id);
            return Mapping.Mapper.Map<Models.Product>(product);
        }

        public async Task<IEnumerable<Models.ProductOption>> GetOptions(Guid productId)
        {
            IEnumerable<ProductOption> productOptions = await productRepository.GetOptions(productId);
            return productOptions.Select(x => Mapping.Mapper.Map<Models.ProductOption>(x));
        }

        public async Task<IEnumerable<Domain.Models.ProductOption>> GetOptionById(Guid productId, Guid optionId)
        {
            var productOptions = await productRepository.GetOptionById(productId, optionId);
            return productOptions.Select(x => Mapping.Mapper.Map<Models.ProductOption>(x));
        }

        public async Task<Models.Product> AddProduct(Models.Product product)
        {
            Data.Product addedProduct = await productRepository.AddProduct(Mapping.Mapper.Map<Data.Product>(product));
            return Mapping.Mapper.Map<Models.Product>(addedProduct);
        }

        public async Task<Models.ProductOption> AddProductOption(Models.ProductOption productOption)
        {
            // Business Logic here
            ProductOption AddedProductOption = await productRepository.AddProductOption(Mapping.Mapper.Map<ProductOption>(productOption));
            return Mapping.Mapper.Map<Models.ProductOption>(AddedProductOption);
        }

        public async Task<Models.Product> UpdateProduct(Guid id, Models.Product product)
        {
            Data.Product updatedProduct = await productRepository.UpdateProduct(id, Mapping.Mapper.Map<Data.Product>(product));
            return Mapping.Mapper.Map<Models.Product>(updatedProduct);
        }

        public async Task<Models.ProductOption> UpdateProductOption(Guid id, Guid optionId, Models.ProductOption productOption)
        {
            ProductOption updatedProductOption = await productRepository.UpdateProductOption(id, optionId, Mapping.Mapper.Map<ProductOption>(productOption));
            return Mapping.Mapper.Map<Models.ProductOption>(updatedProductOption);
        }

        public async Task<Models.Product> DeleteProduct(Guid id)
        {
            Data.Product deletedProduct = await productRepository.DeleteProduct(id);
            return Mapping.Mapper.Map<Models.Product>(deletedProduct);
        }

        public async Task<Models.ProductOption> DeleteProductOption(Guid productId, Guid ProductOptionId)
        {
            ProductOption deletedProductOption = await productRepository.DeleteProductOption(productId, ProductOptionId); ;
            return Mapping.Mapper.Map<Models.ProductOption>(deletedProductOption);
        }
    }
}
