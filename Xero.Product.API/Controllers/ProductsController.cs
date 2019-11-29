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
        private readonly Domain.ProductService productService;

        public ProductsController(ProductContext context, IMapper mapper)
        {
            _mapper = mapper;
            productService = new Domain.ProductService(new ProductRepository(context));
        }

        // GET api/products
        [HttpGet]
        public async Task<ActionResult<Models.Products>> Get([FromQuery] string name)
        {

            IEnumerable<Domain.Models.ProductData> result = await productService.GetAllProducts(name);
            List<Models.ProductData> products = _mapper.Map<List<Domain.Models.ProductData>, List<Models.ProductData>>(result.ToList());
            return Ok(new Models.Products(products));
        }

        // GET api/product/8f2e9176-35ee-4f0a-ae55-83023d2db1a3
        [HttpGet("{id}")]
        public async Task<ActionResult<Models.ProductData>> Get(Guid id)
        {
            Domain.Models.ProductData product = await productService.GetProduct(id);
            if (product == null)
                return NotFound($"Product id {id} not found ");
            Models.ProductData result = _mapper.Map<Models.ProductData>(product);
            return Ok(result); 
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<Models.ProductData>> PostProduct(Models.ProductData product)
        {
            var newProduct = _mapper.Map<Domain.Models.ProductData>(product);

            Domain.Models.ProductData result = await productService.AddProduct(newProduct);
            var addedProduct = _mapper.Map<Models.ProductData>(result);
            return CreatedAtAction("PostProduct", new { id = addedProduct.Id }, addedProduct);
        }

        [HttpPost("{productId}/options")]
        public async Task<ActionResult<Domain.Models.ProductOption>> PostProductOption(Models.ProductOption productOption)
        {
            var newProductOption = _mapper.Map<Domain.Models.ProductOption>(productOption);
            Domain.Models.ProductOption result = await productService.AddProductOption(newProductOption);

            var addedProductOption = _mapper.Map<Models.ProductOption>(result);
            return CreatedAtAction("PostProductOption", new { id = addedProductOption.Id }, addedProductOption);
        }

        // PUT api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] Models.ProductData product)
        {
            var updatedProduct = _mapper.Map<Domain.Models.ProductData>(product);
            Domain.Models.ProductData result = await productService.UpdateProduct(id, updatedProduct);

            return NoContent();

        }

        // PUT api/Products/5/Options/1
        [HttpPut("{id}/options/{optionId}")]
        public async Task<IActionResult> Put(Guid id, Guid optionId, [FromBody] Models.ProductOption productOption)
        {
            var newProductOption = _mapper.Map<Domain.Models.ProductOption>(productOption);
            Domain.Models.ProductOption result = await productService.UpdateProductOption(id, optionId, newProductOption);
            return NoContent();

        }

        // DELETE api/Products/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Models.ProductData>> Delete(Guid id)
        {
            Domain.Models.ProductData result = await productService.DeleteProduct(id);

            if (result == null)
                return NotFound($"No product found with {id} value");

            var deletedProduct = _mapper.Map<Models.ProductData>(result);
            return Ok(deletedProduct);
        }

        // DELETE api/Products/5/options/1
        [HttpDelete("{id}/options/{optionId}")]
        public async Task<ActionResult<Models.ProductOption>> DeleteOption(Guid productId, Guid optionId)
        {
            Domain.Models.ProductOption result = await productService.DeleteProductOption(productId, optionId);
            if (result == null)
                return NotFound($"No product option found with product id {productId} and optionId {optionId}");
            var deletedProductOption = _mapper.Map<Models.ProductOption>(result);
            return Ok(deletedProductOption);
        }

        // GET api/Products/2/options
        [HttpGet("{productId}/options")]
        public async Task<ActionResult<Models.ProductOptions>> GetOptions(Guid productId)
        {
            IEnumerable<Domain.Models.ProductOption> result = await productService.GetOptions(productId);
            if (result == null)
                return NotFound($"No options found with {productId} value");
            List<Models.ProductOption> productOptions = _mapper.Map<List<Domain.Models.ProductOption>, List<Models.ProductOption>>(result.ToList());
            return Ok(new Models.ProductOptions(productOptions));
        }

        // GET api/Products/2/options/3
        [HttpGet("{productId}/options/{optionId}")]
        public async Task<ActionResult<IEnumerable<Models.ProductOption>>> GetOptionById(Guid productId, Guid optionId)
        {
            IEnumerable<Domain.Models.ProductOption> result = await productService.GetOptionById(productId, optionId);
            if (result == null)
                return NotFound($"No product option found with product id {productId} and optionId {optionId}");
            List<Models.ProductOption> productOptions = _mapper.Map<List<Domain.Models.ProductOption>, List<Models.ProductOption>>(result.ToList());
            return Ok(productOptions);
        }
    }
}
