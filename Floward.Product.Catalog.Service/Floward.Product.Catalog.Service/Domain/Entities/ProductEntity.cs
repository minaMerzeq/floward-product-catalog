using System.ComponentModel.DataAnnotations;

namespace Product.Catalog.Service.Domain.Entities
{
    public class ProductEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Cost { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Image { get; set; }
    }
}
