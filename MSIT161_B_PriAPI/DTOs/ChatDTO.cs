namespace MSIT161_B_PriAPI.DTOs
{
    public class ChatDTO
    {
        public int senderId { get; set; }
        public int receiverId { get; set; }
        public string? senderName { get; set; }
        public string? receiverName { get; set; }
        public string? chatimage { get; set; } = null;
        public DateTime? sendTime { get; set; }
        public string? message { get; set; }
        public bool FReport { get; set; }
    }
}
