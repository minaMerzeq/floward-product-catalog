using Product.Catalog.Service.Domain.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Product.Catalog.Service.Services.Interfaces
{
    public interface IProductService
    {
        Task<ActionResult<IEnumerable<ProductReadDto>>> GetAllProducts();

        Task<ActionResult<ProductReadDto>> GetProductById(int id);

        Task<ActionResult<ProductReadDto>> CreateProduct(ProductCreateDto productCreateDto);

        Task<ActionResult<ProductReadDto>> UpdateProduct(int id, ProductCreateDto productCreateDto);

        Task<ActionResult> DeleteProduct(int id);
    }
}
