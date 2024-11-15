using MSIT161_B_PriAPI.Models;

namespace MSIT161_B_PriAPI.DTOs
{
    public class SendMessageDTO
    {
        public int senderId { get; set; }
        public int receiverId { get; set; }
        public int? FChatImageId { get; set; } = null;
        public DateTime? sendTime { get; set; }
        public string? message { get; set; }
    }
}
