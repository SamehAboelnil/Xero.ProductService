using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xero.Product.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductContext context;


        public ProductRepository(ProductContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<ProductData>> GetAllProducts(string name)
        {   
            return string.IsNullOrEmpty(name) ? await context.Product.ToListAsync() : await context.Product.Where(p => p.Name == name).ToListAsync();
        }

        public async Task<ProductData> GetProduct(Guid productId)
        {
            if (!ProductExists(productId))
            {
                throw new ProductNotFoundException($"Product Id{productId} not found");
            }
            return await context.Product.FindAsync(productId);
        }

        public async Task<IEnumerable<ProductOption>> GetOptions(Guid productId)
        {
            List<ProductOption> result = await context.ProductOption.ToListAsync();
            return await context.ProductOption.Where(p => p.ProductId == productId).ToListAsync();
        }

        public async Task<ProductOption> AddProductOption(Guid productId, ProductOption productOption)
        {
            if (!ProductExists(productId))
            {
                throw new ProductNotFoundException($"Product Id{productId} not found");
            }

            var product = await context.Product.FindAsync(productId);
            product.AddOption(productOption);
            await context.SaveChangesAsync();

            return productOption;
        }


        public async Task<ProductData> AddProduct(ProductData product)
        {
            context.Product.Add(product);
            await context.SaveChangesAsync();

            return product;
        }

        public async Task<ProductData> UpdateProduct(Guid productId, ProductData product) // should return product?
        {

            if (!ProductExists(productId))
            {
                throw new ProductNotFoundException($"Product Id{productId} not found");
            }

            context.Entry(product).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) //TODO better exception handling
            {
                throw; 
            }

            return product;
        }

        public async Task<ProductData> DeleteProduct(Guid productId)
        {
            if (!ProductExists(productId))
            {
                throw new ProductNotFoundException($"Product Id{productId} not found");
            }

            ProductData product = await context.Product.FindAsync(productId);
            context.Product.Remove(product);
            await context.SaveChangesAsync();

            return product;
        }

        private bool ProductExists(Guid id)
        {
            return context.Product.Any(e => e.Id == id);
        }

        private bool ProductOptionExists(Guid id)
        {
            return context.ProductOption.Any(e => e.Id == id);
        }

        public async Task<IEnumerable<ProductOption>> GetOptionById(Guid productId, Guid OptionId)
        {
            if (!ProductExists(productId))
            {
                throw new ProductNotFoundException();
            }

            if (!ProductOptionExists(OptionId))
            {
                throw new ProductOptionNotFoundException();
            }
            List<ProductOption> result = await context.ProductOption.ToListAsync();
            return await context.ProductOption.Where(p => p.Id == OptionId && p.ProductId == productId).ToListAsync();
        }

        public async Task<ProductOption> UpdateProductOption(Guid id, Guid optionId, ProductOption productOption)
        {
            if (!ProductExists(id))
            {
                throw new ProductNotFoundException();
            }

            if (!ProductOptionExists(optionId))
            {
                throw new ProductOptionNotFoundException();
            }

            context.Entry(productOption).State = EntityState.Modified;

            try
            {
                var product = await context.Product.FindAsync(id);
                var option = product.Options.Where(x => x.Id == optionId).FirstOrDefault();
                option = productOption;

                
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return productOption;
        }

        public async Task<ProductOption> DeleteProductOption(Guid productId, Guid productOptionId)
        {
            if (!ProductExists(productId))
            {
                throw new ProductNotFoundException();
            }

            if (!ProductOptionExists(productOptionId))
            {
                throw new ProductOptionNotFoundException();
            }

            ProductOption productOption = await context.ProductOption.Where(p => p.ProductId == productId && p.Id == productOptionId).FirstOrDefaultAsync();
            context.ProductOption.Remove(productOption);
            await context.SaveChangesAsync();

            return productOption;
        }

    }
}
