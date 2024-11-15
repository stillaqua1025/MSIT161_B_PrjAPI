namespace MSIT161_B_PriAPI.DTOs
{
    public class AspNetUserDto
    {
        public int BfUserId { get; set; }  // 來自 TUser 表的外鍵
        public string Email { get; set; }
        public string Password { get; set; }  // 前端傳遞純文字密碼
        public string PhoneNumber { get; set; }

    }
}
