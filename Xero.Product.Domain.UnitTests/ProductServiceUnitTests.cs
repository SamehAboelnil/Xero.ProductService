using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xero.Product.Data;
using Xero.Product.Domain.Domain;

namespace Xero.Product.Domain.UnitTests
{
    [TestClass]
    public class ProductControlUnitTests
    {
        Guid newGuid = new Guid("d6c21ad4-8e98-40ed-aa13-b8807051dee6");
        public Guid NewGuid { get => newGuid; set => newGuid = value; }
        [TestMethod]
        public async Task Get_Return_Products_WHEN_DataRepo_Have_Products()
        {
            List<Data.ProductData> resultWithOneRecord = new List<Data.ProductData>
            {
                new Data.ProductData { DeliveryPrice = 2, Description = "Test", Id = NewGuid, Name = " TestName", Price = 123 }
            };

            var productsResponse = new List<Domain.ProductData>();
            productsResponse.Add(
                new Domain.ProductData { DeliveryPrice = 2, Description = "Test", Id = newGuid, Name = " TestName", Price = 123 }
            );

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(repo => repo.GetAllProducts(It.IsAny<string>()))
                .Returns(Task.FromResult(resultWithOneRecord.AsEnumerable()));
            var productService = new ProductService(mockProductRepository.Object);

            var response = await productService.GetAllProducts("name");

            response.Should().BeEquivalentTo(productsResponse);
        }

        [TestMethod]
        public async Task GetProduct_Return_Product_WHEN_Product_is_Exist()
        {
            var resultWithOneRecord = new Data.ProductData { DeliveryPrice = 2, Description = "Test", Id = NewGuid, Name = " TestName", Price = 123 };

            var productsResponse = new Domain.ProductData { DeliveryPrice = 2, Description = "Test", Id = newGuid, Name = " TestName", Price = 123 };

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(repo => repo.GetProduct(It.IsAny<Guid>()))
                .Returns(Task.FromResult(resultWithOneRecord));

            mockProductRepository
               .Setup(repo => repo.IsProductExist(It.IsAny<Guid>()))
               .Returns(Task.FromResult(true));

            var productService = new ProductService(mockProductRepository.Object);

            var response = await productService.GetProduct(newGuid);

            response.Should().BeEquivalentTo(productsResponse);
        }

        [TestMethod]
        [ExpectedException(typeof(ProductOptionNotFoundException))]
        public async Task GetProduct_Throw_Exception_WHEN_Product_is_not_Exist()
        {
            var resultWithOneRecord = new Data.ProductData { DeliveryPrice = 2, Description = "Test", Id = NewGuid, Name = " TestName", Price = 123 };

            var productsResponse = new Domain.ProductData { DeliveryPrice = 2, Description = "Test", Id = newGuid, Name = " TestName", Price = 123 };

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(repo => repo.IsProductExist(It.IsAny<Guid>()))
                .Returns(Task.FromResult(false));
            var productService = new ProductService(mockProductRepository.Object);
            await productService.GetOptionById(newGuid, newGuid);
        }

        [TestMethod]
        public async Task GetProductOption_Return_ProductOption_WHEN_ProductOption_is_Exist()
        {
            var resultWithOneRecord = new Data.ProductOption { ProductId = NewGuid, Name = "ProductOption", Description = " Any desciption ", Id = NewGuid };

            var productOptionResponse = new Domain.ProductOption { ProductId = NewGuid, Name = "ProductOption", Description = " Any desciption ", Id = NewGuid };

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(repo => repo.GetOptionById(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult(resultWithOneRecord));
            mockProductRepository
                .Setup(repo => repo.IsProductOptionExist(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult(true));
            var productService = new ProductService(mockProductRepository.Object);
            var response = await productService.GetOptionById(newGuid, newGuid);
            response.Should().BeEquivalentTo(productOptionResponse);
        }
    }
}
