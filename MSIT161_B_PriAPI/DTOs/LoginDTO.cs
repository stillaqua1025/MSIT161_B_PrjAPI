using System.ComponentModel.DataAnnotations;

namespace MSIT161_B_PriAPI.DTOs
{
    public class LoginDTO
    {

        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string RecaptchaToken { get; set; } // 新增 reCAPTCHA token
    }
}
