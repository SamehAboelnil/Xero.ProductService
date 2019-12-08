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
        public async Task Get_Return_Products_OK_WHEN_DataRepo_Have_Products()
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
        public async Task GetById_RETURN_Not_Found_WHEN_DataRepo_Have_No_Products()
        {
            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(repo => repo.IsProductExist(It.IsAny<Guid>()))
                .Returns(Task.FromResult(false));

            var result1 = await new Controllers.ProductsController(mockProductRepository.Object, Mapper).Get(NewGuid);
            var statusCode = result1.Result;
            Assert.IsInstanceOfType(statusCode, typeof(NotFoundObjectResult));

        }

        [TestMethod]
        public async Task GetById_Return_Product_OK_WHEN_DataRepo_HaveProducts()
        {
            Models.ProductData productResponse = new API.Models.ProductData { DeliveryPrice = 2, Description = "Test", Id = NewGuid, Name = " TestName", Price = 123 };
            ProductData resultOneProduct = new ProductData { DeliveryPrice = 2, Description = "Test", Id = NewGuid, Name = " TestName", Price = 123 };

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(repo => repo.GetProduct(It.IsAny<Guid>()))
                .Returns(Task.FromResult(resultOneProduct));
            mockProductRepository
               .Setup(repo => repo.IsProductExist(It.IsAny<Guid>()))
               .Returns(Task.FromResult(true));


            var result1 = await new Controllers.ProductsController(mockProductRepository.Object, Mapper).Get(NewGuid);
            var statusCode = result1.Result;
            Assert.IsInstanceOfType(statusCode, typeof(OkObjectResult));

            var result = statusCode as OkObjectResult;
            var response = result.Value;
            response.Should().BeEquivalentTo(productResponse);

        }

        [TestMethod]
        public async Task GetById_RETURN_ProductOption_OK_WHEN_DataRepo_HAVE_THE_PRODUCT_OPTION()
        {
            Models.ProductOption productOptionResponse = new Models.ProductOption { ProductId = NewGuid, Name = "ProductOption", Description = " Any desciption ", Id = NewGuid};
            ProductOption resultOneProduct = new ProductOption { ProductId = NewGuid, Name = "ProductOption", Description = " Any desciption ", Id = NewGuid };

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(repo => repo.GetOptionById(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult(resultOneProduct));
            mockProductRepository
               .Setup(repo => repo.IsProductOptionExist(It.IsAny<Guid>(), It.IsAny<Guid>()))
               .Returns(Task.FromResult(true));
            var result1 = await new Controllers.ProductsController(mockProductRepository.Object, Mapper).GetOptionById(NewGuid, NewGuid);
            var statusCode = result1.Result;
            Assert.IsInstanceOfType(statusCode, typeof(OkObjectResult));

            var result = statusCode as OkObjectResult;
            var response = result.Value;
            response.Should().BeEquivalentTo(productOptionResponse);

        }

        [TestMethod]
        public async Task Get_ProductOpiton_ById_RETURN_Not_Found_When_DataRepo_Have_No_ProductsOptions()
        {
            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(repo => repo.IsProductOptionExist(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult(false));

            var result1 = await new Controllers.ProductsController(mockProductRepository.Object, Mapper).GetOptionById(NewGuid, NewGuid);
            var statusCode = result1.Result;
            Assert.IsInstanceOfType(statusCode, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task GetAll_ProductOptions_RETURN_ProductsOptions_OK_WHERE_There_Are_ProductOptions()
        {
            List<ProductOption> resultWithOneRecord = new List<ProductOption>
            {
                new ProductOption { ProductId = NewGuid, Name = "ProductOption", Description = " Any desciption ", Id = NewGuid}
            };

            Models.ProductOptions productsOptionsResponse = new API.Models.ProductOptions(new List<API.Models.ProductOption>()
            {
                new API.Models.ProductOption { ProductId = NewGuid, Name = "ProductOption", Description = " Any desciption ", Id = NewGuid}
            });

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(repo => repo.GetOptions(It.IsAny<Guid>()))
                .Returns(Task.FromResult(resultWithOneRecord.AsEnumerable()));

            var statusCode = (await new Controllers.ProductsController(mockProductRepository.Object, Mapper).GetOptions(NewGuid)).Result;
            Assert.IsInstanceOfType(statusCode, typeof(OkObjectResult));

            var result = statusCode as OkObjectResult;
            var response = result.Value;
            response.Should().BeEquivalentTo(productsOptionsResponse);


        }
    }
}
