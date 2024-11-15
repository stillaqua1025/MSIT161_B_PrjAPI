namespace MSIT161_B_PriAPI.DTOs
{
    public class OrderInfoDTO
    {
        public DateTime time {  get; set; }
        public string finished {  get; set; }
        public DateTime? finishedTime { get; set; }
        public string shipState {  get; set; }
        public int price {  get; set; }
    }
}
