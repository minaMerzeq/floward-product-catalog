using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IWebHostEnvironment _webHostEnvironment;
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
            _webHostEnvironment = A.Fake<IWebHostEnvironment>();
            _sut = new ProductService(_productRepoFake, _mapperFake, _webHostEnvironment, _managerFake);
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
            int id = 1;
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
        public async Task DeleteProduct_ShouldReturnOkResponse_WhenValidId()
        {
            //Arrange
            int id = 1;
            var fakeProduct = A.Dummy<ProductEntity>();
            A.CallTo(() => _productRepoFake.DeleteProduct(fakeProduct)).Returns(Task.FromResult(true));

            //Act
            var actionResult = await _sut.GetProductById(id);

            //Assert
            actionResult.Should().NotBeNull();
            actionResult.Result.Should().BeAssignableTo<OkObjectResult>();
        }
    }
}
