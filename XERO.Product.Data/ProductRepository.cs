﻿using Microsoft.EntityFrameworkCore;
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
            var product =  await context.Product.FindAsync(productId);
            var options = await context.ProductOption.Where(option => option.ProductId == productId).ToListAsync();
            product.Options = options;
            return product;
        }

        public async Task<bool> IsProductExist(Guid productId)
        {
            return await context.Product.AnyAsync(product => product.Id == productId);
        }

        public async Task<bool> IsProductOptionExist(Guid productId, Guid optionId)
        {
            return await IsProductExist(productId) && await context.ProductOption.AnyAsync(option => option.Id == optionId);
        }

        public async Task<IEnumerable<ProductOption>> GetOptions(Guid productId)
        {
            List<ProductOption> result = await context.ProductOption.ToListAsync();
            return await context.ProductOption.Where(p => p.ProductId == productId).ToListAsync();
        }

        public async Task<ProductOption> AddProductOption(Guid productId, ProductOption productOption)
        {
            ProductData product = await context.Product.FindAsync(productId);
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

        public async Task<ProductData> UpdateProduct(Guid productId, ProductData product)
        {
            context.Entry(product).State = EntityState.Modified;
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return product;
        }

        public async Task<ProductData> DeleteProduct(Guid productId)
        {
            ProductData product = await context.Product.FindAsync(productId);
            context.Product.Remove(product);
            await context.SaveChangesAsync();
            return product;
        }

        public async Task<ProductOption> GetOptionById(Guid productId, Guid optionId)
        {
            List<ProductOption> result = await context.ProductOption.ToListAsync();
            return await context.ProductOption.Where(p => p.Id == optionId && p.ProductId == productId).FirstAsync();
        }

        public async Task<ProductOption> UpdateProductOption(Guid id, Guid optionId, ProductOption productOption)
        {
            context.Entry(productOption).State = EntityState.Modified;
            try
            {
                ProductData product = await context.Product.FindAsync(id);
                ProductOption option = product.Options.Where(x => x.Id == optionId).FirstOrDefault();
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
            {
                ProductOption productOption = await context.ProductOption.Where(p => p.ProductId == productId && p.Id == productOptionId).FirstOrDefaultAsync();
                context.ProductOption.Remove(productOption);
                await context.SaveChangesAsync();
                return productOption;
            }
        }

    }
}