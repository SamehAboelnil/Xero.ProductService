using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Xero.Product.Data;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;

namespace Xero.Product.API.UnitTests
{
    [TestClass]
    public class ProductControlUnitTests
    {
        IMapper Mapper;
        Guid newGuid = new Guid("d6c21ad4-8e98-40ed-aa13-b8807051dee6");
        public Guid NewGuid { get => newGuid; set => newGuid = value; }

        readonly ProductData noResult = new ProductData() { };

        [TestInitialize]
        public void Initalize()
        {

            Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
            {
                MapperConfiguration config = new MapperConfiguration(cfg =>
                {
                    cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                    cfg.AddProfile<ModelsProfile>();
                });
                IMapper mapper = config.CreateMapper();
                return mapper;
            });

            Mapper = Lazy.Value;
        }
        [TestMethod]
        public async Task Get_Return_Products_OK_When_DataRepo_Have_Products()
        {
            List<ProductData> resultWithOneRecord = new List<ProductData>
            {
                new ProductData { DeliveryPrice = 2, Description = "Test", Id = NewGuid, Name = " TestName", Price = 123 }
            };

            Models.Products productsResponse = new API.Models.Products(new List<Models.ProductData>()
            {
                new API.Models.ProductData { DeliveryPrice = 2, Description = "Test", Id = newGuid, Name = " TestName", Price = 123 }
            });

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(repo => repo.GetAllProducts(It.IsAny<string>()))
                .Returns(Task.FromResult(resultWithOneRecord.AsEnumerable()));

            var statusCode = (await new Controllers.ProductsController(mockProductRepository.Object, Mapper).Get("TestName")).Result;
            Assert.IsInstanceOfType(statusCode, typeof(OkObjectResult));

            var result = statusCode as OkObjectResult;
            var response = result.Value;
            response.Should().BeEquivalentTo(productsResponse);


        }

        [TestMethod]
        public async Task Get_BY_ID_Return_Not_Found_When_DataRepo_Have_No_Products()
        {
            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(repo => repo.GetProduct(It.IsAny<Guid>()))
                .Throws(new ProductNotFoundException());

            var result1 = await new Controllers.ProductsController(mockProductRepository.Object, Mapper).Get(NewGuid);
            var statusCode = result1.Result;
            Assert.IsInstanceOfType(statusCode, typeof(NotFoundObjectResult));

        }

        [TestMethod]
        public async Task Get_BY_ID_Return_Product_OK_Wehn_DataRepo_Have_No_Products()
        {
            Models.ProductData productResponse = new API.Models.ProductData { DeliveryPrice = 2, Description = "Test", Id = NewGuid, Name = " TestName", Price = 123 };
            ProductData resultOneProduct = new ProductData { DeliveryPrice = 2, Description = "Test", Id = NewGuid, Name = " TestName", Price = 123 };


            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(repo => repo.GetProduct(It.IsAny<Guid>()))
                .Returns(Task.FromResult(resultOneProduct));

            var result1 = await new Controllers.ProductsController(mockProductRepository.Object, Mapper).Get(NewGuid);
            var statusCode = result1.Result;
            Assert.IsInstanceOfType(statusCode, typeof(OkObjectResult));

            var result = statusCode as OkObjectResult;
            var response = result.Value;
            response.Should().BeEquivalentTo(productResponse);




        }
    }
}
