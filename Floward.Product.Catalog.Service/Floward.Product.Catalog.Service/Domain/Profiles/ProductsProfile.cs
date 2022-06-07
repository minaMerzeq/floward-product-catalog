using AutoMapper;
using Floward.Product.Catalog.Service.Domain.Dtos;
using Floward.Product.Catalog.Service.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Floward.Product.Catalog.Service.Domain.Profiles
{
    public class ProductsProfile : Profile
    {
        public ProductsProfile()
        {
            // source --> target
            CreateMap<ProductEntity, ProductReadDto>();
        }
    }
}
