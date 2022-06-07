using Floward.Product.Catalog.Service.Domain.Dtos;
using Floward.Product.Catalog.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Floward.Product.Catalog.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductReadDto>>> GetAllProducts()
        {
            return await _productService.GetAllProducts();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductReadDto>> GetProductById(int id)
        {
            return await _productService.GetProductById(id);
        }

        [HttpPost]
        public async Task<ActionResult<ProductReadDto>> CreateProduct([FromForm] ProductCreateDto productCreateDto)
        {
            return ModelState.IsValid ? await _productService.CreateProduct(productCreateDto) : BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductReadDto>> UpdateProduct(int id, [FromForm] ProductCreateDto productCreateDto)
        {
            return ModelState.IsValid ? await _productService.UpdateProduct(id, productCreateDto) : BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            return await _productService.DeleteProduct(id);
        }
    }
}
