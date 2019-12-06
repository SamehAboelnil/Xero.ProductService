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
        private readonly IProductRepository productRepository;

        public ProductsController(IProductRepository productRepository, IMapper mapper)
        {
            _mapper = mapper;
            this.productRepository = productRepository;
            productService = new Domain.ProductService(productRepository);
        }

        // GET api/products
        [HttpGet]
        public async Task<ActionResult<Models.Products>> Get([FromQuery] string name)
        {

            IEnumerable<Domain.Domain.ProductData> result = await productService.GetAllProducts(name);
            List<Models.ProductData> products = _mapper.Map<List<Domain.Domain.ProductData>, List<Models.ProductData>>(result.ToList());
            return Ok(new Models.Products(products));
        }

        // GET api/product/8f2e9176-35ee-4f0a-ae55-83023d2db1a3
        [HttpGet("{id}")]
        public async Task<ActionResult<Models.ProductData>> Get(Guid id)
        {
            try
            {
                Domain.Domain.ProductData product = await productService.GetProduct(id);
                Models.ProductData result = _mapper.Map<Models.ProductData>(product);
                return Ok(result);
            }
            catch (ProductNotFoundException)
            {
                return NotFound($"No product found with {id} value");
            }
            catch (Exception)
            {
                return StatusCode(500, "Server error");
            }
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<Models.ProductData>> PostProduct(Models.ProductData product)
        {
            try
            {
                var newProduct = _mapper.Map<Domain.Domain.ProductData>(product);

                Domain.Domain.ProductData result = await productService.AddProduct(newProduct);
                var addedProduct = _mapper.Map<Models.ProductData>(result);
                return CreatedAtAction("PostProduct", new { id = addedProduct.Id }, addedProduct);
            }
            catch
            {
                return StatusCode(500, "Server error");
            }
        }

        [HttpPost("{id}/options")]
        public async Task<ActionResult<Domain.Domain.ProductOption>> PostProductOption(Guid id, Models.ProductOption productOption)
        {
            try
            {
                var newProductOption = _mapper.Map<Domain.Domain.ProductOption>(productOption);
                Domain.Domain.ProductOption result = await productService.AddProductOption(id, newProductOption);

                var addedProductOption = _mapper.Map<Models.ProductOption>(result);
                return CreatedAtAction("PostProductOption", new { id = addedProductOption.Id }, addedProductOption);
            }
            catch (ProductNotFoundException)
            {
                return NotFound($"No product found with {id} value");
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        // PUT api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] Models.ProductData product)
        {
            try
            {
                var updatedProduct = _mapper.Map<Domain.Domain.ProductData>(product);
                Domain.Domain.ProductData result = await productService.UpdateProduct(id, updatedProduct);

                return NoContent();
            }
            catch (ProductNotFoundException)
            {
                return NotFound($"No product found with {id} value");
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        // PUT api/Products/5/Options/1
        [HttpPut("{id}/options/{optionId}")]
        public async Task<IActionResult> Put(Guid id, Guid optionId, [FromBody] Models.ProductOption productOption)
        {
            try
            {
                var newProductOption = _mapper.Map<Domain.Domain.ProductOption>(productOption);
                Domain.Domain.ProductOption result = await productService.UpdateProductOption(id, optionId, newProductOption);
                return NoContent();
            }
            catch (ProductNotFoundException)
            {
                return NotFound($"No product found with {id} value");
            }
            catch (ProductOptionNotFoundException)
            {
                return NotFound($"No product option found with {optionId} value");
            }
            catch (Exception error)
            {
                return StatusCode(500, error);
            }

        }

        // DELETE api/Products/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Models.ProductData>> Delete(Guid id)
        {
            try
            {
                Domain.Domain.ProductData result = await productService.DeleteProduct(id);
                var deletedProduct = _mapper.Map<Models.ProductData>(result);
                return Ok(deletedProduct);
            }
            catch(ProductNotFoundException)
            {
                return NotFound($"No product found with {id} value");
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        // DELETE api/Products/5/options/1
        [HttpDelete("{id}/options/{optionId}")]
        public async Task<ActionResult<Models.ProductOption>> DeleteOption(Guid id, Guid optionId)
        {
            try
            {
                Domain.Domain.ProductOption result = await productService.DeleteProductOption(id, optionId);
                var deletedProductOption = _mapper.Map<Models.ProductOption>(result);
                return Ok(deletedProductOption);
            }
            catch(ProductNotFoundException)
            {
                return NotFound($"No product found with {id} value");
            }
            catch (ProductOptionNotFoundException)
            {
                return NotFound($"No product option found with {optionId} value");
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

        }

        // GET api/Products/2/options
        [HttpGet("{id}/options")]
        public async Task<ActionResult<Models.ProductOptions>> GetOptions(Guid id)
        {
            try
            {
                IEnumerable<Domain.Domain.ProductOption> result = await productService.GetOptions(id);
                if (result == null)
                    return NotFound($"No options found with {id} value");
                List<Models.ProductOption> productOptions = _mapper.Map<List<Domain.Domain.ProductOption>, List<Models.ProductOption>>(result.ToList());
                return Ok(new Models.ProductOptions(productOptions));
            }
            catch (ProductNotFoundException)
            {
                return NotFound($"No product found with {id} value");
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        // GET api/Products/2/options/3
        [HttpGet("{id}/options/{optionId}")]
        public async Task<ActionResult<IEnumerable<Models.ProductOption>>> GetOptionById(Guid id, Guid optionId)
        {
            try
            {
                IEnumerable<Domain.Domain.ProductOption> result = await productService.GetOptionById(id, optionId);
                if (result == null)
                    return NotFound($"No product option found with product id {id} and optionId {optionId}");
                List<Models.ProductOption> productOptions = _mapper.Map<List<Domain.Domain.ProductOption>, List<Models.ProductOption>>(result.ToList());
                return Ok(productOptions);
            }
            catch (ProductNotFoundException)
            {
                return NotFound($"No product found with {id} value");
            }
            catch (ProductOptionNotFoundException)
            {
                return NotFound($"No product option found with {optionId} value");
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

        }
    }
}
