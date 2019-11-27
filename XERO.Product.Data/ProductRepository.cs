using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xero.Product.Data
{
    public class ProductRepository : IProductRepository, IDisposable
    {
        private readonly ProductContext context;

        public ProductRepository(ProductContext context)
        {
            this.context = context;
        }

        public void Dispose()
        {
            Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<IEnumerable<Product>> GetAllProducts(string name)
        {
            return string.IsNullOrEmpty(name) ? await context.Product.ToListAsync() : await context.Product.Where(p => p.Name == name).ToListAsync();
        }

        public async Task<Product> GetProduct(Guid id)
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


        public async Task<Product> AddProduct(Product product)
        {
            context.Product.Add(product);
            await context.SaveChangesAsync();

            return product;
        }

        public async Task<Product> UpdateProduct(Guid id, Product product) // should return product?
        {

            if (!ProductExists(id))
            {
                throw new Exception("Not Found");
            }

            context.Entry(product).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException exception) //TODO better exception handling
            {
                throw exception; //todo pattern or custom expceiotn
            }

            return product;
        }

        public async Task<Product> DeleteProduct(Guid id)
        {
            Product product = await context.Product.FindAsync(id);
            if (product == null)
            {
                throw new Exception("Not found exception");
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
                throw new Exception("Product Not Found");
            }

            if (!ProductOptionExists(optionId))
            {
                throw new Exception("Product Option not found");
            }

            context.Entry(productOption).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException exception) //TODO better exception handling
            {
                throw exception; //todo pattern or custom expceiotn
            }

            return productOption;
        }

        public async Task<ProductOption> DeleteProductOption(Guid productId, Guid productOptionId)
        {
            ProductOption productOption = await context.ProductOption.Where(p => p.ProductId == productId && p.Id == productOptionId).FirstOrDefaultAsync();
            if (productOption == null)
            {
                throw new Exception("Not found exception");
            }
            context.ProductOption.Remove(productOption);
            await context.SaveChangesAsync();

            return productOption;
        }
    }
}
