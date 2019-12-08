using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Xero.Product.Data.UnitTests
{
    [TestClass]
    public class RepositoryUnitTests
    {
        [TestMethod]
        public async Task AddProduct_OneProduct_ProductAddedToDB()
        {
            DbContextOptions<ProductContext> options = new DbContextOptionsBuilder<ProductContext>()
               .UseInMemoryDatabase(databaseName: "Products_DB")
               .Options;
            ProductData newProduct;
            using (ProductContext context = new ProductContext(options))
            {
                ProductRepository service = new ProductRepository(context);
                newProduct = new ProductData() { Id = new System.Guid(), DeliveryPrice = 123, Description = "Test", Name = "product1", Options = null, Price = 1231 };
                await service.AddProduct(newProduct);
                context.SaveChanges();
            }
            using (ProductContext context = new ProductContext(options))
            {
                Assert.AreEqual(1, await context.Product.CountAsync());
                newProduct.Should().BeEquivalentTo(await context.Product.SingleAsync());
            }
        }
    }
}


