using AutoMapper;
using BussinessLayer.Repos;
using DataLayer.Models;
using DataLayer.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace eStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepo;
        private readonly IMapper _mapper;

        public ProductController(IProductRepository productRepo, IMapper mapper)
        {
            _productRepo = productRepo;
            _mapper = mapper;
        }
        [HttpGet("list")]
        public IActionResult Gets()
        {
            var products = _productRepo.GetProducts();
            if (products == null || products.Count() == 0)
            {
                return BadRequest("Empty data");
            }
            return Ok(products);
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var product = _productRepo.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
        [HttpGet("search/{unitPrice}")]
        public IActionResult Get(string? productName, double? unitPrice)
        {

            if (productName != null && unitPrice == 0)
            {
                var product = _productRepo.SearchProductsByName(productName);
                if (product == null)
                {
                    return NotFound();
                }
                return Ok(product);
            }
            else if (productName == null && unitPrice != null)
            {
                var product = _productRepo.SearchProductsByUnitPrice((double) unitPrice);
                if (product == null)
                {
                    return NotFound();
                }
                return Ok(product);
            }
            else
            {
                return BadRequest("Only search with productName or UnitPrice");
            }
        }
        [HttpPost("create")]
        public IActionResult Post([FromBody] ProductViewModel product)
        {
            try
            {
                var createProduct = _mapper.Map<Product>(product);
                _productRepo.AddProduct(createProduct);
                return Created("", createProduct);
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }
        [HttpPut("update")]
        public IActionResult Put([FromBody] ProductViewModel product)
        {
            try
            {
                var updateProduct = new Product();
                _mapper.Map<ProductViewModel, Product>(product, updateProduct);
                _productRepo.UpdateProduct(updateProduct);
                return Ok();
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }
        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var product = _productRepo.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                _productRepo.RemoveProduct(product);
                return Ok();
            }
        }
    }
}
