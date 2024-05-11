namespace chic_lighting.DTOs
{
    public class OrderDTO
    {
        public List<ProductQuantityDTO> ProductInfor { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int Payment { get; set; }
        public string Note { get; set; }
    }
}
