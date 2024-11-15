using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MSIT161_B_PriAPI.DTOs;
using System.Reflection;
using MSIT161_B_PriAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using NuGet.Common;
using Google.Apis.Auth;

namespace MSIT161_B_PriAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly dbMSTI161_B_ProjectContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration; // 用來存取 JWT 設定
        private readonly ILogger<IdentityController> _logger;  // 日誌記錄器
        private readonly IMemoryCache _memoryCache;
        public IdentityController(
            UserManager<IdentityUser> userManager , 
            dbMSTI161_B_ProjectContext context ,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration,
            ILogger<IdentityController> logger, 
            IMemoryCache memoryCache) 
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
            _configuration = configuration;
            _logger = logger;
            _memoryCache = memoryCache;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            try
            {
                // 檢查 reCAPTCHA token 是否存在
                if (string.IsNullOrEmpty(loginDTO.RecaptchaToken))
                {
                    return BadRequest(new { message = "reCAPTCHA token 缺失" });
                }

                // 驗證 reCAPTCHA
                var isCaptchaValid = await VerifyRecaptcha(loginDTO.RecaptchaToken);
                if (!isCaptchaValid)
                {
                    return Unauthorized(new { message = "reCAPTCHA 驗證失敗" });
                }

                // 檢查模型是否有效
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { message = "提交的資料不正確" });
                }

                // 使用 UserManager 查詢 AspNetUser 表中的使用者
                //ASP.NET Core Identity 框架的 UserManager 是用來操作和驗證 AspNetUser 表中的資料的。
                //UserManager.FindByEmailAsync() 這個方法就是用來從 AspNetUser 表中查找對應的使用者，並檢查該使用者是否存在。
                var user = await _userManager.FindByEmailAsync(loginDTO.Email);
                if (user == null)
                {
                    return Unauthorized(new { message = "帳號或密碼錯誤" });
                }
                // 檢查密碼是否正確(Asp)
                //var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);
                //if (!result.Succeeded)
                //{
                //    return Unauthorized(new { message = "帳號或密碼錯誤" });
                //}
                // 以下為手動檢查密碼是否正確(非Asp)
                var passwordVerificationResult = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDTO.Password);
                if (passwordVerificationResult != PasswordVerificationResult.Success)
                {
                    Console.WriteLine("Password verification failed for user: " + user.Email);
                    return Unauthorized(new { message = "帳號或密碼錯誤" });
                }
                else
                {
                    Console.WriteLine("Password verification succeeded for user: " + user.Email);
                }


                // 在登入成功後，從 AspNetUsers 和 tUsers 進行 join，獲取完整的使用者資訊
                var fullUserInfo = await _context.AspNetUsers
                    .Include(u => u.BfUser) // 確保 BfUser 是正確的導覽屬性
                    .Where(u => u.Email == loginDTO.Email)
                    .Select(u => new
                    {
                        u.BfUser.FUserId,
                        u.BfUser.FName,
                        u.BfUser.FGender,
                        u.BfUser.FBirthDate,
                        u.BfUser.FAddress,
                        u.BfUser.FUserLevel,
                        u.BfUser.FInvitationCode,
                        u.BfUser.FRegistrationTime,
                        u.BfUser.FUpdatedTime,
                        u.BfUser.FUserIsSeller,
                        u.BfUser.FProfileImage,
                        u.Email,
                        u.PhoneNumber
                    })
                    .FirstOrDefaultAsync();

                if (fullUserInfo == null)
                {
                    return NotFound(new { message = "無法查找到完整的使用者資訊" });
                }             

                // 成功登入後生成 JWT Token，並將 FUserId 傳遞給 GenerateJwtToken 方法
                var token = GenerateJwtToken(user, fullUserInfo.FUserId);

                // 保存 token 到 MemoryCache
                _memoryCache.Set(fullUserInfo.FUserId, token, TimeSpan.FromHours(1));

                // 返回 200 狀態碼並附帶 Token 和使用者資訊
                return Ok(new
                {
                    token = token,
                    userDetails = fullUserInfo
                });
            }
            catch (Exception ex)
            {
                // 捕捉錯誤並返回詳細的錯誤訊息
                return StatusCode(500, new { message = "伺服器錯誤", error = ex.Message });
            }
        }

        private async Task<bool> VerifyRecaptcha(string recaptchaToken)
        {
            var recaptchaVerifyUrl = "https://www.google.com/recaptcha/api/siteverify";
            var secretKey = "6LeZ0z4qAAAAAPAWqIERnyuOHNCMzHfm3AyTz8J5"; // 從配置中讀取你的 reCAPTCHA secret key

            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("secret", secretKey),
                    new KeyValuePair<string, string>("response", recaptchaToken)
            });

                var response = await client.PostAsync(recaptchaVerifyUrl, content);
                var responseString = await response.Content.ReadAsStringAsync();

                var recaptchaResult = JsonConvert.DeserializeObject<RecaptchaResponseDTO>(responseString);

                // 只需檢查 recaptchaResult.Success 對於 reCAPTCHA v2
                if (recaptchaResult.Success)
                {
                    return true; // 驗證成功
                }
                else
                {
                    // 印錯誤代碼幫助調試
                    if (recaptchaResult.ErrorCodes != null)
                    {
                        Console.WriteLine("reCAPTCHA 驗證失敗，錯誤代碼：" + string.Join(", ", recaptchaResult.ErrorCodes));
                    }
                    return false; // 驗證失敗
                }
            }
        }

        [HttpPost("login/google")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginDto googleLoginDTO)
        {
            if (googleLoginDTO == null || string.IsNullOrEmpty(googleLoginDTO.Token))
            {
                return BadRequest(new { message = "無效的 Google Token" });
            }

            try
            {
                // 檢查 Google Token 是否存在
                if (string.IsNullOrEmpty(googleLoginDTO.Token))
                {
                    return BadRequest(new { message = "Google Token 缺失" });
                }

                // 驗證 Google Token
                var validPayload = await VerifyGoogleToken(googleLoginDTO.Token);
                if (validPayload == null)
                {
                    return Unauthorized(new { message = "Google Token 無效" });
                }

                // 使用 UserManager找資料庫中的會員（基於 Google 的 Email）
                var user = await _userManager.FindByEmailAsync(validPayload.Email);
                if (user == null)
                {
                    // 如果User不存在，可以選擇註冊或回傳錯誤
                    return Unauthorized(new { message = "該Email尚未註冊過，請註冊" });
                }

                // 找到對應的 AspNetUser 和 BfUser 資料
                var fullUserInfo = await _context.AspNetUsers
                    .Include(u => u.BfUser)
                    .Where(u => u.Email == validPayload.Email)
                    .FirstOrDefaultAsync();

                if (fullUserInfo == null)
                {
                    return NotFound(new { message = "無法找到完整的會員資料" });
                }

                // 更新 BfUser 的 FName 為 Google 的 FamilyName 和 GivenName
                fullUserInfo.BfUser.FName = $"{validPayload.FamilyName}{validPayload.GivenName}";

                // 保存更新的資料到資料庫
                _context.AspNetUsers.Update(fullUserInfo);
                await _context.SaveChangesAsync();

                // 成功更新後生成 JWT Token 並返回
                var token = GenerateJwtToken(user, fullUserInfo.BfUser.FUserId);

                // 保存 token 到 MemoryCache
                _memoryCache.Set(fullUserInfo.BfUser.FUserId, token, TimeSpan.FromHours(1));

                // 返回更新後的會員資料和 Token
                return Ok(new
                {
                    token = token,
                    userDetails = new
                    {
                        fullUserInfo.BfUser.FUserId,
                        fullUserInfo.BfUser.FName,
                        fullUserInfo.BfUser.FGender,
                        fullUserInfo.BfUser.FBirthDate,
                        fullUserInfo.BfUser.FAddress,
                        fullUserInfo.BfUser.FUserLevel,
                        fullUserInfo.BfUser.FInvitationCode,
                        fullUserInfo.BfUser.FRegistrationTime,
                        fullUserInfo.BfUser.FUpdatedTime,
                        fullUserInfo.BfUser.FUserIsSeller,
                        fullUserInfo.BfUser.FProfileImage,
                        fullUserInfo.Email,
                        fullUserInfo.PhoneNumber
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "伺服器錯誤", error = ex.Message });
            }
        }

        // 驗證 Google Token 的方法
        [NonAction]
        private async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(string token)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string> { "754360782793-j76k4tiuqkrc8ja4q56moggsfkioi85d.apps.googleusercontent.com" }
                };

                // 驗證 Google 的 JWT Token
                var payload = await GoogleJsonWebSignature.ValidateAsync(token, settings);
                return payload;
            }
            catch
            {
                return null; // 如果驗證失敗，回傳 null
            }
        }

        [NonAction]
        private string GenerateJwtToken(IdentityUser user, int fUserId)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("FUserId", fUserId.ToString()) // 加入 FUserId 作為 Claim
            };

            var tokenExpiration = int.Parse(_configuration["Jwt:TokenExpirationInMinutes"]);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(tokenExpiration),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        [Authorize]
        [HttpPost("updateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDTO userUpdateDTO)
        {
            try
            {
                // 從 JWT Token 取得使用者的 ID
                var fUserIdClaim = HttpContext.User.FindFirst("FUserId");
                if (fUserIdClaim == null)
                {
                    return Unauthorized(new { message = "無法從 JWT Token 取得使用者資訊" });
                }

                var fUserId = int.Parse(fUserIdClaim.Value);

                // 從資料庫取得目前使用者的完整資料
                var user = await _context.TUsers.FirstOrDefaultAsync(u => u.FUserId == fUserId);
                if (user == null)
                {
                    return NotFound(new { message = "使用者未找到" });
                }
                var olduser = await _context.TUsers.FirstOrDefaultAsync(u => u.FUserId == fUserId);
                // 更新允許變更的欄位
                user.FName = userUpdateDTO.fName;
                user.FGender = userUpdateDTO.fGender;
                user.FBirthDate = userUpdateDTO.fBirthDate;
                user.FAddress = userUpdateDTO.fAddress;
                user.FProfileImage = olduser.FProfileImage;

                // 更新 fUpdatedTime 為當下時間
                user.FUpdatedTime = DateTime.Now;

                //解追蹤
                _context.Entry(olduser).State = EntityState.Detached;

                // 儲存變更
                _context.TUsers.Update(user);
                await _context.SaveChangesAsync();

                return Ok(new { message = "使用者資料更新成功" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "伺服器錯誤", error = ex.Message });
            }
        }


        [HttpPost("registertUser")]
        public async Task<IActionResult> RegistertUser([FromBody] TUser registertUser) //使用 [FromBody] 綁定資料時，模型驗證系統會自動檢查傳入的資料是否合法
                                                                                       //亦可透過 ModelState.IsValid 來判斷驗證結果。
        {
            // 檢查模型是否有效
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "提交的資料不正確" });
            }

            // 將圖片欄位設置為 NULL，暫不處理圖片上傳
            registertUser.FProfileImage = "/uploads/images/man.PNG";

            // Create User
            var userToCreate = new TUser
            {
                FName = registertUser.FName,
                FGender = registertUser.FGender ?? "不提供",
                FBirthDate = registertUser.FBirthDate,
                FAddress = registertUser.FAddress,
                FUserLevel = 1, // 新會員預設一般會員
                FInvitationCode = GenerateInvitationCode(),
                FRegistrationTime = DateTime.Now,
                FUpdatedTime = DateTime.Now,
                FUserIsSeller = 1, // 預設每個會員為賣家
                FProfileImage = registertUser.FProfileImage // 儲存圖片路徑或為 null
            };

            // 儲存資料
            try
            {
                _context.TUsers.Add(userToCreate);
                await _context.SaveChangesAsync();

                // 將 FUserId 保存到 MemoryCache，設置一個唯一的鍵
                _memoryCache.Set("FUserId", userToCreate.FUserId, TimeSpan.FromHours(1));


                return Ok(new { message = "註冊成功" });

                // return Ok(new { bfUserId = userToCreate.FUserId }); // 返回新會員的 bfUserId
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new { message = "資料庫錯誤，請稍後再試", error = ex.InnerException?.Message });
            }
            catch (Exception ex)
            {
                // 捕捉內部異常並返回給前端
                return StatusCode(500, new { message = "伺服器錯誤，請聯繫管理員", error = ex.Message });
                //return BadRequest(new { message = "註冊會員失敗", error = ex.InnerException?.Message ?? ex.Message });
            }
        }

       
        //產生驗證碼
        private string GenerateInvitationCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        [HttpPost("registerAspNetUser")]
        public async Task<IActionResult> RegisterAspNetUser([FromBody] AspNetUserDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("提交的資料不正確");
            }

            // 檢查是否存在這個 Email
            var existingUser = await _context.AspNetUsers.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (existingUser != null)
            {
                return BadRequest("這個 Email 已經被註冊過了");
            }

            // 從 MemoryCache 中獲取 FUserId
            if (_memoryCache.TryGetValue("FUserId", out int bfUserId))
            {
                // 創建新的 AspNetUsers 資料
                var aspNetUser = new AspNetUser
                {
                    BfUserId = bfUserId,  // 使用從快取中獲取的 FUserId
                    Email = model.Email,
                    UserName = model.Email,  // UserName 可以設置為 Email
                    NormalizedUserName = model.Email.ToUpper(),  // UserName 的正規化版本
                    NormalizedEmail = model.Email.ToUpper(),  // Email 的正規化版本
                    PasswordHash = HashPassword(model.Password),  // 使用純文字密碼進行 Hash
                    EmailConfirmed = false,
                    PhoneNumber = model.PhoneNumber,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                    SecurityStamp = Guid.NewGuid().ToString(),  // SecurityStamp 用於驗證和密碼重置等操作
                    ConcurrencyStamp = Guid.NewGuid().ToString() // ConcurrencyStamp 用於併發控制
                };

                try
                {
                    _context.AspNetUsers.Add(aspNetUser);
                    await _context.SaveChangesAsync();
                    return Ok(aspNetUser);  // 成功後返回 AspNetUser 資料
                }
                catch (Exception ex)
                {
                    // 記錄錯誤並返回適當的錯誤訊息
                    _logger.LogError(ex, "An error occurred while registering user");
                    return BadRequest(new { message = "註冊失敗", error = ex.InnerException?.Message ?? ex.Message });
                }
            }
            else
            {
                return BadRequest("找不到會員的 ID");
            }            
        }


        [NonAction]
        public string HashPassword(string password)
        {
            var passwordHasher = new PasswordHasher<AspNetUser>();
            return passwordHasher.HashPassword(null, password);
        }

        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            if (string.IsNullOrEmpty(model.UserId) || string.IsNullOrEmpty(model.Token) || string.IsNullOrEmpty(model.NewPassword))
            {
                return BadRequest("請提供完整的資料");
            }

            // 查找用戶
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound("找不到用戶");
            }

            // 驗證重設密碼的 token 是否有效
            var isTokenValid = await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, "ResetPassword", model.Token);
            if (!isTokenValid)
            {
                return BadRequest("重設密碼的 token 無效或已過期");
            }

            // 嘗試重設密碼
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (result.Succeeded)
            {
                return Ok(new { message = "密碼重設成功" });
            }

            // 返回具體的錯誤訊息
            var errors = result.Errors.Select(e => e.Description).ToList();
            return BadRequest(new { message = "密碼重設失敗", errors });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // 從 JWT Token 中提取 FUserId（這裡是你登錄時存儲到 Token 的 FUserId）
            var fUserIdClaim = HttpContext.User.FindFirst("FUserId");
            if (fUserIdClaim == null)
            {
                return Unauthorized(new { message = "無法從 JWT Token 中取得使用者資訊" });
            }

            // 從 MemoryCache 中檢索 token
            var fUserId = int.Parse(fUserIdClaim.Value);

            // 從快取中檢查是否存在此 fUserId 的 token
            if (_memoryCache.TryGetValue(fUserId, out string cachedToken))
            {
                // 如果存在，從 MemoryCache 中移除該 token
                _memoryCache.Remove(fUserId);

                // 返回成功消息
                return Ok(new { message = "用戶已成功登出" });
            }
            else
            {
                // 如果找不到 token，返回未授權
                return Unauthorized(new { message = "未找到對應的 token，可能已經登出" });
            }
        }

    }
}
