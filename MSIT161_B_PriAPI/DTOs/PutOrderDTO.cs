namespace MSIT161_B_PriAPI.DTOs
{
    public class PutOrderDTO
    {
        public int price { get; set; }
        public int? couponId { get; set; }
        public string address { get; set; }
        public int userId {  get; set; }
    }
}
