using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Product.Catalog.Service.Domain.Dtos;
using Product.Catalog.Service.Domain.Entities;
using Product.Catalog.Service.Domain.Profiles;
using Product.Catalog.Service.Domain.RabbitMQ.Interfaces;
using Product.Catalog.Service.Domain.Repos.Interfaces;
using Product.Catalog.Service.Services.Implementation;
using Product.Catalog.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Product.Catalog.Service.Tests
{
    public class ProductServiceTests
    {
        private readonly IProductRepo _productRepoFake;
        private readonly IMapper _mapperFake;
        private readonly IImageService _imageService;
        private readonly IRabbitManager _managerFake;

        private readonly IProductService _sut;

        public ProductServiceTests()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ProductsProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            _mapperFake = mapper;
            
            _productRepoFake = A.Fake<IProductRepo>();
            _managerFake = A.Fake<IRabbitManager>();
            _imageService = A.Fake<IImageService>();
            _sut = new ProductService(_productRepoFake, _mapperFake, _imageService, _managerFake);
        }

        [Fact]
        public async Task GetAllProducts_ShouldReturnOkResponse_WhenDataFound()
        {
            //Arrange
            int count = 5;
            var fakeProducts = A.CollectionOfDummy<ProductEntity>(count).AsEnumerable();
            A.CallTo(() => _productRepoFake.GetAllProducts()).Returns(Task.FromResult(fakeProducts));

            //Act
            var actionResult = await _sut.GetAllProducts();

            //Assert
            actionResult.Should().NotBeNull();
            actionResult.Result.Should().BeAssignableTo<OkObjectResult>();
            actionResult.Result.As<OkObjectResult>().Value
                .Should()
                .NotBeNull()
                .And.BeOfType(typeof(List<ProductReadDto>));

            Assert.Equal(count, actionResult.Result.As<OkObjectResult>().Value.As<IEnumerable<ProductReadDto>>().Count());
        }

        [Fact]
        public async Task GetProductById_ShouldReturnOkResponse_WhenDataFound()
        {
            //Arrange
            int id = A.Dummy<int>();
            var fakeProduct = A.Dummy<ProductEntity>();
            A.CallTo(() => _productRepoFake.GetProductById(id)).Returns(Task.FromResult(fakeProduct));

            //Act
            var actionResult = await _sut.GetProductById(id);

            //Assert
            actionResult.Should().NotBeNull();
            actionResult.Result.Should().BeAssignableTo<OkObjectResult>();
            actionResult.Result.As<OkObjectResult>().Value
                .Should()
                .NotBeNull()
                .And.BeOfType(typeof(ProductReadDto));
        }

        [Fact]
        public async Task GetProductById_ShouldReturnNotFound_WhenNoDataFound()
        {
            //Arrange
            int id = A.Dummy<int>();
            ProductEntity fakeProduct = null;
            A.CallTo(() => _productRepoFake.GetProductById(id)).Returns(Task.FromResult(fakeProduct));

            //Act
            var actionResult = await _sut.GetProductById(id);

            //Assert
            actionResult.Should().NotBeNull();
            actionResult.Result.Should().BeAssignableTo<NotFoundResult>();
        }

        [Fact]
        public async Task CreateProduct_ShouldReturnCreated_WhenValidRequest()
        {
            //Arrange
            var fakeProduct = A.Dummy<ProductEntity>();
            var fakeProductCreateDto = A.Dummy<ProductCreateDto>();
            var image = A.Dummy<IFormFile>();
            A.CallTo(() => _productRepoFake.CreateProduct(fakeProduct)).Returns(Task.FromResult(fakeProduct));
            A.CallTo(() => _imageService.UploadImage(image)).Returns("image.jpg");
            A.CallTo(() => _managerFake.Publish("", "dummy-exchange", "topic", "dummy.key.*")).DoesNothing();

            //Act
            var actionResult = await _sut.CreateProduct(fakeProductCreateDto);

            //Assert
            actionResult.Should().NotBeNull();
            actionResult.Result.Should().BeAssignableTo<CreatedResult>();
            actionResult.Result.As<CreatedResult>().Value
                .Should()
                .NotBeNull()
                .And.BeOfType(typeof(ProductReadDto));
        }

        [Fact]
        public async Task UpdateProduct_ShouldReturnOkResponse_WhenValidRequest()
        {
            //Arrange
            int id = A.Dummy<int>();
            var fakeProduct = A.Dummy<ProductEntity>();
            var fakeProductCreateDto = A.Dummy<ProductCreateDto>();
            var image = A.Dummy<IFormFile>();
            A.CallTo(() => _productRepoFake.UpdateProduct(fakeProduct)).Returns(Task.FromResult(fakeProduct));
            A.CallTo(() => _productRepoFake.GetProductById(id)).Returns(Task.FromResult(fakeProduct));
            A.CallTo(() => _imageService.UploadImage(image)).Returns("image.jpg");

            //Act
            var actionResult = await _sut.UpdateProduct(id, fakeProductCreateDto);

            //Assert
            actionResult.Should().NotBeNull();
            actionResult.Result.Should().BeAssignableTo<OkObjectResult>();
            actionResult.Result.As<OkObjectResult>().Value
                .Should()
                .NotBeNull()
                .And.BeOfType(typeof(ProductReadDto));
        }

        [Fact]
        public async Task UpdateProduct_ShouldReturnNotFound_WhenInvalidId()
        {
            //Arrange
            int id = A.Dummy<int>();
            ProductEntity fakeProduct = null;
            var fakeProductCreateDto = A.Dummy<ProductCreateDto>();
            A.CallTo(() => _productRepoFake.GetProductById(id)).Returns(Task.FromResult(fakeProduct));

            //Act
            var actionResult = await _sut.UpdateProduct(id, fakeProductCreateDto);

            //Assert
            actionResult.Should().NotBeNull();
            actionResult.Result.Should().BeAssignableTo<NotFoundResult>();
        }

        [Fact]
        public async Task DeleteProduct_ShouldReturnOkResponse_WhenValidId()
        {
            //Arrange
            int id = A.Dummy<int>();
            var fakeProduct = A.Dummy<ProductEntity>();
            A.CallTo(() => _productRepoFake.GetProductById(id)).Returns(Task.FromResult(fakeProduct));
            A.CallTo(() => _productRepoFake.DeleteProduct(fakeProduct)).Returns(Task.FromResult(true));

            //Act
            var actionResult = await _sut.DeleteProduct(id);

            //Assert
            actionResult.Should().NotBeNull();
            actionResult.Should().BeAssignableTo<OkResult>();
        }

        [Fact]
        public async Task DeleteProduct_ShouldReturnNotFound_WhenInvalidId()
        {
            //Arrange
            int id = A.Dummy<int>();
            ProductEntity fakeProduct = null;
            A.CallTo(() => _productRepoFake.GetProductById(id)).Returns(Task.FromResult(fakeProduct));

            //Act
            var actionResult = await _sut.DeleteProduct(id);

            //Assert
            actionResult.Should().NotBeNull();
            actionResult.Should().BeAssignableTo<NotFoundResult>();
        }
    }
}
