using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Xero.Product.Data;
using Xero.Product.API;
using Xero.Product.API;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Xero.Product.API.UnitTests
{
    [TestClass]
    public class ProductControlUnitTests
    {
        [TestMethod]
        public async Task Get_Return_Products_OK_Wehn_DataRepo_Have_Products()
        {
            var result = new List<ProductData>();
            result.Add(new ProductData { DeliveryPrice = 2, Description = "Test", Id = Guid.NewGuid(), Name = " TestName", Price = 123 });
            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(repo => repo.GetAllProducts(It.IsAny<string>()))
                .Returns(Task.FromResult(result.AsEnumerable()));


            Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
            {
                MapperConfiguration config = new MapperConfiguration(cfg =>
                {
                    // This line ensures that internal properties are also mapped over.
                    cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                    cfg.AddProfile<ModelsProfile>();
                });
                IMapper mapper = config.CreateMapper();
                return mapper;
            });

            IMapper Mapper = Lazy.Value;

            Xero.Product.API.Controllers.ProductsController productsController = new API.Controllers.ProductsController(mockProductRepository.Object, Mapper);
            var result1 = await productsController.Get("TestName");

            
            var statusCode = result1.Result;
            Assert.IsInstanceOfType(statusCode, typeof(OkObjectResult));

        }
    }
}
