namespace Floward.Product.Catalog.Service.Domain.Dtos
{
    public class ProductReadDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Cost { get; set; }

        public decimal Price { get; set; }

        public string Image { get; set; }
    }
}
