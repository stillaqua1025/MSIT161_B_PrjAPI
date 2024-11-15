using Microsoft.AspNetCore.Mvc;

namespace MSIT161_B_PriAPI.Providers
{
    public class JwtService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult GetfUserIDfromJWT(out int fUserId)
        {
            // 初始化 fUserId 預設值
            fUserId = 0;

            // 從 JWT Token 中取得使用者的 fUserId
            var fUserIdClaim = _httpContextAccessor.HttpContext.User.FindFirst("FUserId");
            if (fUserIdClaim == null)
            {
                // 無法從 JWT Token 中取得使用者資訊時返回 401 錯誤
                return new UnauthorizedObjectResult(new { message = "無法從 JWT Token 取得使用者資訊" });
            }

            // 嘗試解析 fUserId
            if (!int.TryParse(fUserIdClaim.Value, out fUserId))
            {
                // 如果解析失敗，返回錯誤
                return new BadRequestObjectResult(new { message = "無效的使用者 ID" });
            }

            // 如果成功解析，返回 null 表示沒有錯誤
            return null;
        }
    }
}
