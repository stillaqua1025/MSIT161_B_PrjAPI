using Microsoft.Build.Framework;

namespace MSIT161_B_PriAPI.DTOs
{
    public class TUserDto
    {
        [Required]
        public string FName { get; set; }

        [Required]
        public string FGender { get; set; }

        [Required]
        public DateTime? FBirthDate { get; set; }

        [Required]
        public string FAddress { get; set; }

        public IFormFile UserPhoto { get; set; }  // 用於處理圖片上傳
    }

}
