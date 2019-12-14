using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xero.Product.API.Validation;
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
        public async Task<ActionResult<Contracts.Products>> GetProducts([FromQuery] string name)
        {
            IEnumerable<Domain.Domain.ProductData> result = await productService.GetAllProducts(name);
            List<Contracts.ProductData> products = _mapper.Map<List<Domain.Domain.ProductData>, List<Contracts.ProductData>>(result.ToList());
            return Ok(new Contracts.Products(products));
        }

        // GET api/product/8f2e9176-35ee-4f0a-ae55-83023d2db1a3
        [HttpGet("{id}")]
        public async Task<ActionResult<Contracts.ProductDataDetailed>> GetProductById(Guid id)
        {
            try
            {
                Domain.Domain.ProductData product = await productService.GetProduct(id);
                Contracts.ProductDataDetailed result = _mapper.Map<Contracts.ProductDataDetailed>(product);
                return Ok(result);
            }
            catch (ProductNotFoundException)
            {
                return NotFound($"No product found with {id} value");
            }
            catch (Exception ex)
            {
                // Future work, log errors
                return StatusCode(500, "Server error");
            }
        }

        // POST: api/Products
        // POST: api/Products
        [HttpPost]
        [ModelValidation]
        public async Task<ActionResult<Contracts.ProductDataDetailed>> PostProduct(Contracts.ProductDataDetailed product)
        {
            try
            {
                Domain.Domain.ProductData newProduct = _mapper.Map<Domain.Domain.ProductData>(product);
                Domain.Domain.ProductData result = await productService.AddProduct(newProduct);
                Contracts.ProductDataDetailed addedProduct = _mapper.Map<Contracts.ProductDataDetailed>(result);
                return CreatedAtAction("PostProduct", new { id = addedProduct.Id }, addedProduct);
            }
            catch (ProductDuplicateException)
            {
                return new ConflictResult();
            }
            catch(Exception e)
            {
                return StatusCode(500);
            }
        }

        [HttpPost("{id}/options")]
        [ModelValidation]
        public async Task<ActionResult<Domain.Domain.ProductOption>> PostProductOption(Guid id, Contracts.ProductOption productOption)
        {
            try
            {
                Domain.Domain.ProductOption newProductOption = _mapper.Map<Domain.Domain.ProductOption>(productOption);
                newProductOption.ProductId = id;

                Domain.Domain.ProductOption result = await productService.AddProductOption(id, newProductOption);

                Contracts.ProductOption addedProductOption = _mapper.Map<Contracts.ProductOption>(result);
                return CreatedAtAction("PostProductOption", new { id = addedProductOption.Id }, addedProductOption);
            }
            catch (ProductNotFoundException)
            {
                return NotFound($"No product found with {id} value");
            }
            catch (ProductOptionDuplicateException)
            {
                return new ConflictResult();
            }
            catch(Exception ex)
            {
                return StatusCode(500);
            }
        }

        // PUT api/Products/5
        [HttpPut("{id}")]
        [ModelValidation]
        public async Task<IActionResult> Put(Guid id, [FromBody] Contracts.ProductDataDetailed product)
        {
            try
            {
                Domain.Domain.ProductData updatedProduct = _mapper.Map<Domain.Domain.ProductData>(product);
                Domain.Domain.ProductData result = await productService.UpdateProduct(id, updatedProduct);

                return NoContent();
            }
            catch (ProductNotFoundException)
            {
                return NotFound($"No product found with {id} value");
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        // PUT api/Products/5/Options/1
        [HttpPut("{id}/options/{optionId}")]
        [ModelValidation]
        public async Task<IActionResult> Put(Guid id, Guid optionId, [FromBody] Contracts.ProductOption productOption)
        {
            try
            {
                Domain.Domain.ProductOption newProductOption = _mapper.Map<Domain.Domain.ProductOption>(productOption);
                newProductOption.ProductId = id;

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
#pragma warning disable CS0168 // The variable 'e' is declared but never used
            catch(Exception e)
#pragma warning restore CS0168 // The variable 'e' is declared but never used
            {
                return StatusCode(500);
            }
        }

        // DELETE api/Products/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Contracts.ProductDataDetailed>> Delete(Guid id)
        {
            try
            {
                Domain.Domain.ProductData result = await productService.DeleteProduct(id);
                Contracts.ProductDataDetailed deletedProduct = _mapper.Map<Contracts.ProductDataDetailed>(result);
                return Ok(deletedProduct);
            }
            catch (ProductNotFoundException)
            {
                return NotFound($"No product found with {id} value");
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        // DELETE api/Products/5/options/1
        [HttpDelete("{id}/options/{optionId}")]
        [ModelValidation]
        public async Task<ActionResult<Contracts.ProductOption>> DeleteOption(Guid id, Guid optionId)
        {
            try
            {
                Domain.Domain.ProductOption productOption = await productService.DeleteProductOption(id, optionId);
                Contracts.ProductOption deletedProductOption = _mapper.Map<Contracts.ProductOption>(productOption);
                return Ok(deletedProductOption);
            }
            catch (ProductNotFoundException)
            {
                return NotFound($"No product found with {id} value");
            }
            catch (ProductOptionNotFoundException)
            {
                return NotFound($"No product option found with {optionId} value");
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }

        }

        // GET api/Products/2/options
        [HttpGet("{id}/options")]
        public async Task<ActionResult<Contracts.ProductOptions>> GetOptions(Guid id)
        {
            try
            {
                IEnumerable<Domain.Domain.ProductOption> result = await productService.GetOptions(id);
                if (result == null)
                {
                    return NotFound($"No options found with {id} value");
                }

                List<Contracts.ProductOption> productOptions = _mapper.Map<List<Domain.Domain.ProductOption>, List<Contracts.ProductOption>>(result.ToList());
                return Ok(new Contracts.ProductOptions(productOptions));
            }
            catch (ProductNotFoundException)
            {
                return NotFound($"No product found with {id} value");
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        // GET api/Products/2/options/3
        [HttpGet("{id}/options/{optionId}")]
        public async Task<ActionResult<IEnumerable<Contracts.ProductOption>>> GetOptionById(Guid id, Guid optionId)
        {
            try
            {
                Domain.Domain.ProductOption result = await productService.GetOptionById(id, optionId);
                Contracts.ProductOption productOption = _mapper.Map<Contracts.ProductOption>(result);
                return Ok(productOption);
            }
            catch (ProductNotFoundException)
            {
                return NotFound($"No product found with {id} value");
            }
            catch (ProductOptionNotFoundException)
            {
                return NotFound($"No product option found with {optionId} value");
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }

        }
    }
}
