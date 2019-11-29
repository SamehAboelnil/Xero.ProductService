using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xero.Product.Data;

namespace Xero.Product.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly Domain.ProductData productData;

        public ProductsController(ProductContext context, IMapper mapper)
        {
            _mapper = mapper;
            productData = new Domain.ProductData(new ProductRepository(context));
        }

        // GET api/products
        [HttpGet]
        public async Task<ActionResult<Models.Products>> Get([FromQuery] string name)
        {

            IEnumerable<Domain.Models.ProductData> result = await productData.GetAllProducts(name);
            List<Models.ProductData> products = _mapper.Map<List<Domain.Models.ProductData>, List<Models.ProductData>>(result.ToList());
            return Ok(new Models.Products(products));
        }

        // GET api/product/8f2e9176-35ee-4f0a-ae55-83023d2db1a3
        [HttpGet("{id}")]
        public async Task<ActionResult<Models.ProductData>> Get(Guid id)
        {
            Domain.Models.ProductData product = await productData.GetProduct(id);
            Models.ProductData result = _mapper.Map<Models.ProductData>(product);
            return Ok(result); 
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<Models.ProductData>> PostProduct(Models.ProductData product)
        {
            var newProduct = _mapper.Map<Domain.Models.ProductData>(product);

            Domain.Models.ProductData result = await productData.AddProduct(newProduct);
            var addedProduct = _mapper.Map<Models.ProductData>(result);
            return CreatedAtAction("PostProduct", new { id = addedProduct.Id }, addedProduct);
        }

        [HttpPost("{productId}/options")]
        public async Task<ActionResult<Domain.Models.ProductOption>> PostProductOption(Models.ProductOption productOption)
        {
            var newProductOption = _mapper.Map<Domain.Models.ProductOption>(productOption);
            Domain.Models.ProductOption result = await productData.AddProductOption(newProductOption);

            var addedProductOption = _mapper.Map<Models.ProductOption>(result);
            return CreatedAtAction("PostProductOption", new { id = addedProductOption.Id }, addedProductOption);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] Models.ProductData product)
        {
            var updatedProduct = _mapper.Map<Domain.Models.ProductData>(product);
            Domain.Models.ProductData result = await productData.UpdateProduct(id, updatedProduct);

            return NoContent();

        }

        // PUT api/values/5
        [HttpPut("{id}/options/{optionId}")]
        public async Task<IActionResult> Put(Guid id, Guid optionId, [FromBody] Models.ProductOption productOption)
        {
            var newProductOption = _mapper.Map<Domain.Models.ProductOption>(productOption);
            Domain.Models.ProductOption result = await productData.UpdateProductOption(id, optionId, newProductOption);
            return NoContent();

        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Models.ProductData>> Delete(Guid id)
        {
            Domain.Models.ProductData result = await productData.DeleteProduct(id);
            var deletedProduct = _mapper.Map<Models.ProductData>(result);
            return Ok(deletedProduct);
        }

        [HttpDelete("{id}/options/{optionId}")]
        public async Task<ActionResult<Models.ProductOption>> DeleteOption(Guid id, Guid optionId)
        {
            Domain.Models.ProductOption result = await productData.DeleteProductOption(id, optionId);
            var deletedProductOption = _mapper.Map<Models.ProductOption>(result);
            return Ok(deletedProductOption);
        }

        [HttpGet("{productId}/options")]
        public async Task<ActionResult<Models.ProductOptions>> GetOptions(Guid productId)
        {
            IEnumerable<Domain.Models.ProductOption> result = await productData.GetOptions(productId);
            List<Models.ProductOption> productOptions = _mapper.Map<List<Domain.Models.ProductOption>, List<Models.ProductOption>>(result.ToList());
            return Ok(new Models.ProductOptions(productOptions));
        }

        [HttpGet("{productId}/options/{optionId}")]
        public async Task<ActionResult<IEnumerable<Models.ProductOption>>> GetOptionById(Guid productId, Guid optionId)
        {
            IEnumerable<Domain.Models.ProductOption> result = await productData.GetOptionById(productId, optionId);
            List<Models.ProductOption> productOptions = _mapper.Map<List<Domain.Models.ProductOption>, List<Models.ProductOption>>(result.ToList());
            return Ok(productOptions);
        }
    }
}
