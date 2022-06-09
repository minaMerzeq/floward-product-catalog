using Product.Catalog.Service.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Product.Catalog.Service.Domain.Repos.Interfaces
{
    public interface IProductRepo
    {
        Task<IEnumerable<ProductEntity>> GetAllProducts();

        Task<ProductEntity> GetProductById(int id);

        Task<ProductEntity> UpdateProduct(ProductEntity product);

        Task<ProductEntity> CreateProduct(ProductEntity product);

        Task<bool> DeleteProduct(ProductEntity product);
    }
}
