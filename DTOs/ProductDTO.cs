namespace chic_lighting.DTOs
{
    public class ProductDTO
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public int ProductStatusId { get; set; }
        public int CategoryId { get; set; }
        public double Saleprice { get; set; }
        public IFormFile Image { get; set; }
        public string Description { get; set; }

    }
}
