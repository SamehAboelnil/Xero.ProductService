using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xero.Product.Data;

namespace Xero.Product.Domain
{
    public class ProductService
    {
        private readonly IProductRepository productRepository;
        public ProductService(IProductRepository productContext)
        {
            productRepository = productContext;
        }

        public async Task<IEnumerable<Models.ProductData>> GetAllProducts(string name)
        {
            IEnumerable<Data.ProductData> products = await productRepository.GetAllProducts(name);
            return products.Select(x => Mapping.Mapper.Map<Models.ProductData>(x)).ToList();

        }

        public async Task<Models.ProductData> GetProduct(Guid id)
        {
            Data.ProductData product = await productRepository.GetProduct(id);
            return Mapping.Mapper.Map<Models.ProductData>(product);
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

        public async Task<Models.ProductData> AddProduct(Models.ProductData product)
        {
            Data.ProductData addedProduct = await productRepository.AddProduct(Mapping.Mapper.Map<Data.ProductData>(product));
            return Mapping.Mapper.Map<Models.ProductData>(addedProduct);
        }

        public async Task<Models.ProductOption> AddProductOption(Guid productId, Models.ProductOption productOption)
        {
            // Business Logic here
            ProductOption AddedProductOption = await productRepository.AddProductOption(productId, Mapping.Mapper.Map<ProductOption>(productOption));
            return Mapping.Mapper.Map<Models.ProductOption>(AddedProductOption);
        }

        public async Task<Models.ProductData> UpdateProduct(Guid id, Models.ProductData product)
        {
            Data.ProductData updatedProduct = await productRepository.UpdateProduct(id, Mapping.Mapper.Map<Data.ProductData>(product));
            return Mapping.Mapper.Map<Models.ProductData>(updatedProduct);
        }

        public async Task<Models.ProductOption> UpdateProductOption(Guid id, Guid optionId, Models.ProductOption productOption)
        {
            ProductOption updatedProductOption = await productRepository.UpdateProductOption(id, optionId, Mapping.Mapper.Map<ProductOption>(productOption));
            return Mapping.Mapper.Map<Models.ProductOption>(updatedProductOption);
        }

        public async Task<Models.ProductData> DeleteProduct(Guid id)
        {
            Data.ProductData deletedProduct = await productRepository.DeleteProduct(id);
            return Mapping.Mapper.Map<Models.ProductData>(deletedProduct);
        }

        public async Task<Models.ProductOption> DeleteProductOption(Guid productId, Guid ProductOptionId)
        {
            ProductOption deletedProductOption = await productRepository.DeleteProductOption(productId, ProductOptionId); ;
            return Mapping.Mapper.Map<Models.ProductOption>(deletedProductOption);
        }
    }
}
