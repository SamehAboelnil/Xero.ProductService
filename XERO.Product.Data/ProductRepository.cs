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

        public async Task<ProductData> GetProduct(Guid id)
        {
            return await context.Product.FindAsync(id);
        }

        public async Task<IEnumerable<ProductOption>> GetOptions(Guid productId)
        {
            List<ProductOption> result = await context.ProductOption.ToListAsync();
            return await context.ProductOption.Where(p => p.ProductId == productId).ToListAsync();
        }

        public async Task<ProductOption> AddProductOption(ProductOption productOption)
        {
            context.ProductOption.Add(productOption);
            await context.SaveChangesAsync();

            return productOption; // TODO what should we return
        }


        public async Task<ProductData> AddProduct(ProductData product)
        {
            context.Product.Add(product);
            await context.SaveChangesAsync();

            return product;
        }

        public async Task<ProductData> UpdateProduct(Guid id, ProductData product) // should return product?
        {

            if (!ProductExists(id))
            {
                throw new ProductNotFoundException($"Product Id{id} not found");
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

        public async Task<ProductData> DeleteProduct(Guid id)
        {
            ProductData product = await context.Product.FindAsync(id);
            if (product == null)
            {
                throw new ProductNotFoundException();
            }
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
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) //TODO better exception handling
            {
                throw;
            }

            return productOption;
        }

        public async Task<ProductOption> DeleteProductOption(Guid productId, Guid productOptionId)
        {
            ProductOption productOption = await context.ProductOption.Where(p => p.ProductId == productId && p.Id == productOptionId).FirstOrDefaultAsync();
            if (productOption == null)
            {
                throw new ProductOptionNotFoundException();
            }
            context.ProductOption.Remove(productOption);
            await context.SaveChangesAsync();

            return productOption;
        }
    }
}
