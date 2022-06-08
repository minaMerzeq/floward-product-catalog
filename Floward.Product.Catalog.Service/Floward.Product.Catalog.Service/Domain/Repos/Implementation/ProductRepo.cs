using Product.Catalog.Service.Domain.Entities;
using Product.Catalog.Service.Domain.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Product.Catalog.Service.Domain.Repos.Implementation
{
    public class ProductRepo : IProductRepo
    {
        private readonly AppDbContext _context;

        public ProductRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductEntity>> GetAllProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<ProductEntity> GetProductById(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<ProductEntity> CreateProduct(ProductEntity product)
        {
            try
            {
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                return product;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ProductEntity> UpdateProduct(ProductEntity product)
        {
            try
            {
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                return product;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> DeleteProduct(ProductEntity product)
        {
            try
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
