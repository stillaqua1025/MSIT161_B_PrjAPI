namespace MSIT161_B_PriAPI.DTOs
{
    public class OrderDetailDTO
    {
        public string name { get; set; }
        public int price { get; set; }
        public int? quantity { get; set; }
        public string? imageUrl { get; set; }
    }
}
