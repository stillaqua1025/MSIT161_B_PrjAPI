namespace MSIT161_B_PriAPI.DTOs
{
    public class ShoppingCartDTO
    {
        public int scid { get; set; } //購物車id
        public int? pdid { get; set; } //商品明細id
        public string name { get; set; }
        public int price { get; set; }
        public int? quantity { get; set; }
        public string? imageUrl { get; set; }
    }
}
