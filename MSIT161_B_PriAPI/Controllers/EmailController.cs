using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using MSIT161_B_PriAPI.DTOs;

namespace MSIT161_B_PriAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailSender _emailSender;
        private readonly UserManager<IdentityUser> _userManager;  // 假設你正在使用 ASP.NET Identity

        public EmailController(UserManager<IdentityUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }


        //輸入Email後，發送忘記密碼的信件
        [HttpPost("sendForgetPasswordEmail")]
        public async Task<IActionResult> SendForgetPasswordEmail([FromBody] EmailRequest request)
        {
            if (string.IsNullOrEmpty(request.Email))
            {
                return BadRequest("Email address is required.");
            }

            // 查找用戶
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return NotFound("找不到該電子郵件對應的使用者");
            }

            try
            {
                // 生成重設密碼的 Token
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var userId = user.Id;

                // 構建重設密碼的 URL
                var resetUrl = $"http://localhost:4200/#/resetPassword?userId={userId}&token={Uri.EscapeDataString(resetToken)}";

                // 發送郵件
                await _emailSender.SendEmailAsync(request.Email, "重設密碼",
                    $"點擊以下連結來重設你的密碼：<a href=\"{resetUrl}\">重設密碼</a>");

                return Ok(new { message = "重設密碼郵件已發送" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"郵件發送失敗: {ex.Message}");
            }
        }

        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            Console.WriteLine($"UserId: {model.UserId}");
            Console.WriteLine($"Token: {model.Token}");
            Console.WriteLine($"NewPassword: {model.NewPassword}");

            if (string.IsNullOrEmpty(model.UserId) || string.IsNullOrEmpty(model.Token) || string.IsNullOrEmpty(model.NewPassword))
            {
                return BadRequest("請提供完整的資料");
            }

            // 查找用戶
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound(new { message = "找不到用戶" });
            }

            // 日誌：驗證重設密碼的 token 並嘗試重設密碼
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (result.Succeeded)
            {
                return Ok(new { message = "密碼重設成功" });  // 以 JSON 格式返回
            }

            // 日誌：記錄具體錯誤原因
            foreach (var error in result.Errors)
            {
                Console.WriteLine(error.Description);
            }

            // 返回具體的錯誤訊息
            var errors = result.Errors.Select(e => e.Description).ToList();
            return BadRequest(new { message = "密碼重設失敗", errors });
        }

        [HttpPost("checkEmailExists")]
        public async Task<IActionResult> CheckEmailExists([FromBody] EmailCheckDto emailCheckDto)
        {
            if (string.IsNullOrEmpty(emailCheckDto.Email))
            {
                return BadRequest(new { message = "Email 不得為空" });
            }

            var user = await _userManager.FindByEmailAsync(emailCheckDto.Email);
            if (user != null)
            {
                return Ok(new { exists = true });
            }
            return Ok(new { exists = false });
        }


        public class EmailRequest
        {
            public string Email { get; set; }
        }
    }
}
