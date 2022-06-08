using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Product.Catalog.Service.Domain.Dtos
{
    public class ProductCreateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Cost { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public IFormFile Image { get; set; }
    }
}
