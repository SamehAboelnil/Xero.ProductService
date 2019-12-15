using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xero.Product.Data;

namespace Xero.Product.Domain.UnitTests
{
    [TestClass]
    public class ProductControlUnitTests
    {
        private Guid newGuid = new Guid("d6c21ad4-8e98-40ed-aa13-b8807051dee6");
        public Guid NewGuid { get => newGuid; set => newGuid = value; }
        [TestMethod]
        public async Task GetAllProducts_ReturnProducts()
        {
            List<ProductData> resultWithOneRecord = new List<ProductData>
            {
                new ProductData { DeliveryPrice = 2, Description = "Test", Id = NewGuid, Name = " TestName", Price = 123 }
            };

            List<Domain.ProductData> productsResponse = new List<Domain.ProductData>
            {
                new Domain.ProductData { DeliveryPrice = 2, Description = "Test", Id = newGuid, Name = " TestName", Price = 123, Options = new List<Domain.ProductOption>() }
            };

            Mock<IProductRepository> mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(repo => repo.GetAllProducts(It.IsAny<string>()))
                .Returns(Task.FromResult(resultWithOneRecord.AsEnumerable()));
            ProductService productService = new ProductService(mockProductRepository.Object);

            IEnumerable<Domain.ProductData> response = await productService.GetAllProducts("name");

            response.Should().BeEquivalentTo(productsResponse);
        }

        [TestMethod]
        public async Task GetProduct_ValidProductId_ReturnProduct()
        {
            ProductData resultWithOneRecord = new Data.ProductData { DeliveryPrice = 2, Description = "Test", Id = NewGuid, Name = " TestName", Price = 123, Options = new List<ProductOption>() };

            Domain.ProductData productsResponse = new Domain.ProductData { DeliveryPrice = 2, Description = "Test", Id = newGuid, Name = " TestName", Price = 123, Options = new List<Domain.ProductOption>() };

            Mock<IProductRepository> mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(repo => repo.GetProduct(It.IsAny<Guid>()))
                .Returns(Task.FromResult(resultWithOneRecord));

            mockProductRepository
               .Setup(repo => repo.IsProductExist(It.IsAny<Guid>()))
               .Returns(Task.FromResult(true));

            ProductService productService = new ProductService(mockProductRepository.Object);

            Domain.ProductData response = await productService.GetProduct(newGuid);

            response.Should().BeEquivalentTo(productsResponse);
        }

        [TestMethod]
        [ExpectedException(typeof(ProductOptionNotFoundException))]
        public async Task GetOptionById_OptionNotExist_ThrowProductOptionNotFoundException()
        {
            ProductData resultWithOneRecord = new ProductData { DeliveryPrice = 2, Description = "Test", Id = NewGuid, Name = " TestName", Price = 123 };

            Domain.ProductData productsResponse = new Domain.ProductData { DeliveryPrice = 2, Description = "Test", Id = newGuid, Name = " TestName", Price = 123 };

            Mock<IProductRepository> mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(repo => repo.IsProductExist(It.IsAny<Guid>()))
                .Returns(Task.FromResult(false));
            ProductService productService = new ProductService(mockProductRepository.Object);
            await productService.GetOptionById(newGuid, newGuid);
        }
        [TestMethod]
        public async Task GetOptionById_ReturnOption()
        {
            ProductOption resultWithOneRecord = new Data.ProductOption { ProductId = NewGuid, Name = "ProductOption", Description = " Any desciption ", Id = NewGuid };

            Domain.ProductOption productOptionResponse = new Domain.ProductOption { ProductId = NewGuid, Name = "ProductOption", Description = " Any desciption ", Id = NewGuid };

            Mock<IProductRepository> mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(repo => repo.GetOptionById(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult(resultWithOneRecord));
            mockProductRepository
                .Setup(repo => repo.IsProductOptionExist(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult(true));
            ProductService productService = new ProductService(mockProductRepository.Object);
            Domain.ProductOption response = await productService.GetOptionById(newGuid, newGuid);
            response.Should().BeEquivalentTo(productOptionResponse);
        }
    }
}
