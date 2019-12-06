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
        public ProductService(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task<IEnumerable<Domain.ProductData>> GetAllProducts(string name)
        {
            IEnumerable<Data.ProductData> products = await productRepository.GetAllProducts(name);
            return products.Select(x => Mapping.Mapper.Map<Domain.ProductData>(x)).ToList();

        }

        public async Task<Domain.ProductData> GetProduct(Guid id)
        {
            if (!await ProductExists(id))
            {
                throw new ProductNotFoundException($"Product Id{id} not found");
            }
            Data.ProductData product = await productRepository.GetProduct(id);
            return Mapping.Mapper.Map<Domain.ProductData>(product);
        }

        public async Task<IEnumerable<Domain.ProductOption>> GetOptions(Guid productId)
        {
            IEnumerable<ProductOption> productOptions = await productRepository.GetOptions(productId);
            return productOptions.Select(x => Mapping.Mapper.Map<Domain.ProductOption>(x));
        }

        public async Task<IEnumerable<Product.Domain.Domain.ProductOption>> GetOptionById(Guid productId, Guid optionId)
        {
            if (!await ProductOptionExists(productId, optionId))
            {
                throw new ProductOptionNotFoundException();
            }
            var productOptions = await productRepository.GetOptionById(productId, optionId);
            return productOptions.Select(x => Mapping.Mapper.Map<Domain.ProductOption>(x));
        }

        public async Task<Domain.ProductData> AddProduct(Domain.ProductData product)
        {
            ProductData addedProduct = await productRepository.AddProduct(Mapping.Mapper.Map<Data.ProductData>(product));
            return Mapping.Mapper.Map<Domain.ProductData>(addedProduct);
        }

        public async Task<Domain.ProductOption> AddProductOption(Guid productId, Domain.ProductOption productOption)
        {
            if (!await ProductExists(productId))
            {
                throw new ProductNotFoundException($"Product Id{productId} not found");
            }
            // Business Logic here
            ProductOption AddedProductOption = await productRepository.AddProductOption(productId, Mapping.Mapper.Map<ProductOption>(productOption));
            return Mapping.Mapper.Map<Domain.ProductOption>(AddedProductOption);
        }

        public async Task<Domain.ProductData> UpdateProduct(Guid id, Domain.ProductData product)
        {
            if (!await ProductExists(id))
            {
                throw new ProductNotFoundException($"Product Id{id} not found");
            }
            ProductData updatedProduct = await productRepository.UpdateProduct(id, Mapping.Mapper.Map<Data.ProductData>(product));
            return Mapping.Mapper.Map<Domain.ProductData>(updatedProduct);
        }

        public async Task<Domain.ProductOption> UpdateProductOption(Guid id, Guid optionId, Domain.ProductOption productOption)
        {

            if (!await ProductOptionExists(id, optionId))
            {
                throw new ProductOptionNotFoundException();
            }
            ProductOption updatedProductOption = await productRepository.UpdateProductOption(id, optionId, Mapping.Mapper.Map<ProductOption>(productOption));
            return Mapping.Mapper.Map<Domain.ProductOption>(updatedProductOption);
        }

        public async Task<Domain.ProductData> DeleteProduct(Guid id)
        {
            if (!await ProductExists(id))
            {
                throw new ProductNotFoundException($"Product Id{id} not found");
            }
            Data.ProductData deletedProduct = await productRepository.DeleteProduct(id);
            return Mapping.Mapper.Map<Domain.ProductData>(deletedProduct);
        }

        public async Task<Domain.ProductOption> DeleteProductOption(Guid productId, Guid ProductOptionId)
        {
            if (!await ProductOptionExists(productId, ProductOptionId))
            {
                throw new ProductOptionNotFoundException();
            }
            ProductOption deletedProductOption = await productRepository.DeleteProductOption(productId, ProductOptionId); ;
            return Mapping.Mapper.Map<Domain.ProductOption>(deletedProductOption);
        }


        private async Task<bool> ProductExists(Guid id)
        {
            if (await productRepository.GetProduct(id) == null)
                return false;
            return true;
        }

        private async Task<bool> ProductOptionExists(Guid productId, Guid optionId)
        {
            if (await productRepository.GetOptionById(productId, optionId) == null)
                return false;
            return true;
        }
    }
}
