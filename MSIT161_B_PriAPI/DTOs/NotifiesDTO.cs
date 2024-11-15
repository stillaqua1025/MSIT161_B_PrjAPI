namespace MSIT161_B_PriAPI.DTOs
{
    public class NotifiesDTO
    {
        public int FNotifyId { get; set; }
        public int? FUserId { get; set; }
        public string? notify {  get; set; }
        public int? FNotifyTypeId { get; set; }
        public string? notifytype {  get; set; }
        public bool? FIsNotRead { get; set; }
        public bool? FNotifyState { get; set; }
    }
}
