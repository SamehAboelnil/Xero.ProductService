using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xero.Product.Data;

namespace Xero.Product.API.UnitTests
{
    [TestClass]
    public class ProductControlUnitTests
    {
        private IMapper Mapper;
        private Guid newGuid = new Guid("d6c21ad4-8e98-40ed-aa13-b8807051dee6");
        public Guid NewGuid { get => newGuid; set => newGuid = value; }

        private readonly ProductData noResult = new ProductData() { };

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
        public async Task GetProducts_ReturnProducts()
        {
            List<ProductData> resultWithOneRecord = new List<ProductData>
            {
                new ProductData { DeliveryPrice = 2, Description = "Test", Id = NewGuid, Name = " TestName", Price = 123 }
            };

            Contracts.Products productsResponse = new Contracts.Products(new List<Contracts.ProductData>()
            {
                new Contracts.ProductData { DeliveryPrice = 2, Description = "Test", Id = newGuid, Name = " TestName", Price = 123}
            });

            Mock<IProductRepository> mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(repo => repo.GetAllProducts(It.IsAny<string>()))
                .Returns(Task.FromResult(resultWithOneRecord.AsEnumerable()));

            ActionResult statusCode = (await new Controllers.ProductsController(mockProductRepository.Object, Mapper).GetProducts("TestName")).Result;
            Assert.IsInstanceOfType(statusCode, typeof(OkObjectResult));

            OkObjectResult result = statusCode as OkObjectResult;
            object response = result.Value;
            response.Should().BeEquivalentTo(productsResponse);
        }

        public async Task GetProducts_ReturnEmptyProductsList()
        {
            List<ProductData> resultWithOneRecord = new List<ProductData>{};

            Contracts.Products productsResponse = new Contracts.Products(new List<Contracts.ProductData>(){});

            Mock<IProductRepository> mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(repo => repo.GetAllProducts(It.IsAny<string>()))
                .Returns(Task.FromResult(resultWithOneRecord.AsEnumerable()));

            ActionResult statusCode = (await new Controllers.ProductsController(mockProductRepository.Object, Mapper).GetProducts("TestName")).Result;
            Assert.IsInstanceOfType(statusCode, typeof(OkObjectResult));

            OkObjectResult result = statusCode as OkObjectResult;
            object response = result.Value;
            response.Should().BeEquivalentTo(productsResponse);
        }

        [TestMethod]
        public async Task GetProductByID_IdNotExist_ReturnNotFound()
        {
            Mock<IProductRepository> mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(repo => repo.IsProductExist(It.IsAny<Guid>()))
                .Returns(Task.FromResult(false));

            ActionResult<Contracts.ProductData> result1 = await new Controllers.ProductsController(mockProductRepository.Object, Mapper).GetProductById(NewGuid);
            ActionResult statusCode = result1.Result;
            Assert.IsInstanceOfType(statusCode, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task GetProductByID_IdExist_ReturnProduct()
        {
            Contracts.ProductData productResponse = new Contracts.ProductData { DeliveryPrice = 2, Description = "Test", Id = NewGuid, Name = " TestName", Price = 123 };
            ProductData resultOneProduct = new ProductData { DeliveryPrice = 2, Description = "Test", Id = NewGuid, Name = " TestName", Price = 123 };
            Mock<IProductRepository> mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(repo => repo.GetProduct(It.IsAny<Guid>()))
                .Returns(Task.FromResult(resultOneProduct));
            mockProductRepository
               .Setup(repo => repo.IsProductExist(It.IsAny<Guid>()))
               .Returns(Task.FromResult(true));

            ActionResult<Contracts.ProductData> result1 = await new Controllers.ProductsController(mockProductRepository.Object, Mapper).GetProductById(NewGuid);
            ActionResult statusCode = result1.Result;
            Assert.IsInstanceOfType(statusCode, typeof(OkObjectResult));

            OkObjectResult result = statusCode as OkObjectResult;
            object response = result.Value;
            response.Should().BeEquivalentTo(productResponse);
        }

        [TestMethod]
        public async Task GetOptionById_IdExist_ReturnProductOption()
        {
            Contracts.ProductOption productOptionResponse = new Contracts.ProductOption { Name = "ProductOption", Description = " Any desciption ", Id = NewGuid };
            ProductOption resultOneProduct = new ProductOption { ProductId = NewGuid, Name = "ProductOption", Description = " Any desciption ", Id = NewGuid };

            Mock<IProductRepository> mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(repo => repo.GetOptionById(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult(resultOneProduct));
            mockProductRepository
               .Setup(repo => repo.IsProductOptionExist(It.IsAny<Guid>(), It.IsAny<Guid>()))
               .Returns(Task.FromResult(true));
            ActionResult<IEnumerable<Contracts.ProductOption>> result1 = await new Controllers.ProductsController(mockProductRepository.Object, Mapper).GetOptionById(NewGuid, NewGuid);
            ActionResult statusCode = result1.Result;
            Assert.IsInstanceOfType(statusCode, typeof(OkObjectResult));

            OkObjectResult result = statusCode as OkObjectResult;
            object response = result.Value;
            response.Should().BeEquivalentTo(productOptionResponse);

        }

        [TestMethod]
        public async Task GetOptionById_IdNotExist_ReturnNotFound()
        {
            Mock<IProductRepository> mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(repo => repo.IsProductOptionExist(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult(false));

            ActionResult<IEnumerable<Contracts.ProductOption>> result1 = await new Controllers.ProductsController(mockProductRepository.Object, Mapper).GetOptionById(NewGuid, NewGuid);
            ActionResult statusCode = result1.Result;
            Assert.IsInstanceOfType(statusCode, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task GetAll_ProductOptions_RETURN_ProductsOptions_OK_WHERE_There_Are_ProductOptions()
        {
            List<ProductOption> resultWithOneRecord = new List<ProductOption>
            {
                new ProductOption { ProductId = NewGuid, Name = "ProductOption", Description = " Any desciption ", Id = NewGuid}
            };

            Contracts.ProductOptions productsOptionsResponse = new API.Contracts.ProductOptions(new List<API.Contracts.ProductOption>()
            {
                new Contracts.ProductOption { Name = "ProductOption", Description = " Any desciption ", Id = NewGuid}
            });

            Mock<IProductRepository> mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(repo => repo.GetOptions(It.IsAny<Guid>()))
                .Returns(Task.FromResult(resultWithOneRecord.AsEnumerable()));

            ActionResult statusCode = (await new Controllers.ProductsController(mockProductRepository.Object, Mapper).GetOptions(NewGuid)).Result;
            Assert.IsInstanceOfType(statusCode, typeof(OkObjectResult));

            OkObjectResult result = statusCode as OkObjectResult;
            object response = result.Value;
            response.Should().BeEquivalentTo(productsOptionsResponse);
        }

        [TestMethod]
        public async Task Post_Product_RETURN_Created_WHERE_There_is_no_ID_Duplication()
        {
            ProductData resultOneProduct = new ProductData { DeliveryPrice = 2, Description = "Test", Id = NewGuid, Name = " TestName", Price = 123 };
            Contracts.ProductData product = new Contracts.ProductData { DeliveryPrice = 2, Description = "Test", Id = NewGuid, Name = " TestName", Price = 123};
            Mock<IProductRepository> mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(repo => repo.AddProduct(It.IsAny<ProductData>()))
                .Returns(Task.FromResult(resultOneProduct));
            mockProductRepository
              .Setup(repo => repo.IsProductExist(It.IsAny<Guid>()))
              .Returns(Task.FromResult(false));
            var productController = new Controllers.ProductsController(mockProductRepository.Object, Mapper);
            var response = await productController.PostProduct(product);
            ActionResult statusCode = response.Result;
            Assert.IsInstanceOfType(statusCode, typeof(CreatedAtActionResult));
        }

        [TestMethod]
        public async Task Post_Product_THROW_Exception_WHERE_IDIsDuplicate()
        {
            ProductData resultOneProduct = new ProductData { DeliveryPrice = 2, Description = "Test", Id = NewGuid, Name = " TestName", Price = 123 };
            Contracts.ProductData product = new Contracts.ProductData { DeliveryPrice = 2, Description = "Test", Id = NewGuid, Name = " TestName", Price = 123};
            Mock<IProductRepository> mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(repo => repo.AddProduct(It.IsAny<ProductData>()))
                .Returns(Task.FromResult(resultOneProduct));
            mockProductRepository
              .Setup(repo => repo.IsProductExist(It.IsAny<Guid>()))
              .Returns(Task.FromResult(true));
            var productController = new Controllers.ProductsController(mockProductRepository.Object, Mapper);
            var response = await productController.PostProduct(product);
            ActionResult statusCode = response.Result;
            Assert.IsInstanceOfType(statusCode, typeof(ConflictResult));
        }
    }
}
