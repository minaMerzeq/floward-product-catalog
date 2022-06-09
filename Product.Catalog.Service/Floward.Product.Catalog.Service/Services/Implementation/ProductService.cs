using AutoMapper;
using Product.Catalog.Service.Domain.Dtos;
using Product.Catalog.Service.Domain.Entities;
using Product.Catalog.Service.Domain.RabbitMQ.Interfaces;
using Product.Catalog.Service.Domain.Repos.Interfaces;
using Product.Catalog.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Product.Catalog.Service.Services.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IProductRepo _productRepo;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        private readonly IRabbitManager _manager;

        public ProductService(IProductRepo productRepo, IMapper mapper, IImageService imageService, IRabbitManager manager)
        {
            _productRepo = productRepo;
            _mapper = mapper;
            _imageService = imageService;
            _manager = manager;
        }

        public async Task<ActionResult<IEnumerable<ProductReadDto>>> GetAllProducts()
        {
            var products = await _productRepo.GetAllProducts();
            return new OkObjectResult(_mapper.Map<IEnumerable<ProductReadDto>>(products));
        }

        public async Task<ActionResult<ProductReadDto>> GetProductById(int id)
        {
            var product = await _productRepo.GetProductById(id);
            if (product != null)
            {
                return new OkObjectResult(_mapper.Map<ProductReadDto>(product));
            }

            return new NotFoundResult();
        }

        public async Task<ActionResult<ProductReadDto>> CreateProduct(ProductCreateDto productCreateDto)
        {
            var product = CreateNewProductEntity(productCreateDto);
            var createdProduct = await _productRepo.CreateProduct(product);
            if (createdProduct != null)
            {
                // publish message  
                _manager.Publish(product.Name, "product.exchange", "topic", "product.queue.*");

                return new CreatedResult("/api/Products/" + createdProduct.Id, _mapper.Map<ProductReadDto>(createdProduct));
            }

            // there was error while saving changes
            return new StatusCodeResult(500);
        }

        public async Task<ActionResult<ProductReadDto>> UpdateProduct(int id, ProductCreateDto productCreateDto)
        {
            var product = await _productRepo.GetProductById(id);
            if (product != null)
            {
                product.Name = productCreateDto.Name;
                product.Cost = productCreateDto.Cost;
                product.Price = productCreateDto.Price;
                product.Image = _imageService.UploadImage(productCreateDto.Image);

                product = await _productRepo.UpdateProduct(product);
                if (product != null)
                {
                    return new OkObjectResult(_mapper.Map<ProductReadDto>(product));
                }

                // there was error while saving changes
                return new StatusCodeResult(500);
            }

            return new NotFoundResult();
        }

        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _productRepo.GetProductById(id);
            if (product != null)
            {
                var isDeleted = await _productRepo.DeleteProduct(product);
                if (isDeleted)
                {
                    return new OkResult();
                }

                // there was error while saving changes
                return new StatusCodeResult(500);
            }

            return new NotFoundResult();
        }


        #region Helper Method
        private ProductEntity CreateNewProductEntity(ProductCreateDto productCreateDto)
        {
            return new ProductEntity()
                {
                    Name = productCreateDto.Name,
                    Cost = productCreateDto.Cost,
                    Price = productCreateDto.Price,
                    Image = _imageService.UploadImage(productCreateDto.Image)
                };
        }

        #endregion
    }
}
