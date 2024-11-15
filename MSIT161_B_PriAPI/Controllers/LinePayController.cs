using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static MSIT161_B_PriAPI.DTOs.LinePayDto;
using System.Text;
using Humanizer;
using MSIT161_B_PriAPI.Providers;
using Microsoft.EntityFrameworkCore;
using MSIT161_B_PriAPI.Models;
using MSIT161_B_PriAPI.DTOs;
using System.Net.Http.Headers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MSIT161_B_PriAPI.Hubs;
using MSIT161_B_PriAPI.Repositories;

namespace MSIT161_B_PriAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinePayController : ControllerBase
    {
        private readonly JwtService _jwtService;
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _memoryCache;
        private readonly string channelId = "2006252872";  // 從 LINE 開發者平台取得
        private readonly string channelSecretKey = "c03b980d8b0ef08d680198c2f938f056";  // 從 LINE 開發者平台取得
        private readonly string linePayBaseApiUrl = "https://sandbox-api-pay.line.me";  // Sandbox API URL
        private static readonly Dictionary<string, CheckOutDTO> OrderPayments = new Dictionary<string, CheckOutDTO>();//儲存訂單編號對應的金額
        private static readonly Dictionary<string, string> OrderToken = new Dictionary<string, string>();
        private readonly NotificationService _notificationService;
        private readonly IHubContext<NotificationHub> _hubContext;

        public LinePayController(HttpClient httpClient, JwtService jwtService, IMemoryCache memoryCache, NotificationService notificationService, IHubContext<NotificationHub> hubContext)
        {
            _httpClient = httpClient;
            _jwtService = jwtService;
            _memoryCache = memoryCache;
            _hubContext = hubContext;
            _notificationService = notificationService;
        }

        [HttpPost]
        public async Task<IActionResult> RequestPayment([FromBody] CheckOutDTO checkOutDto)
        {
            // 使用 JwtService 來獲取 fUserId
            var result = _jwtService.GetfUserIDfromJWT(out int fUserId);
            if (result != null)
            {
                // 如果有錯誤，根據具體情況返回適當的 ActionResult
                if (result is UnauthorizedResult)
                {
                    return Unauthorized(); // 返回 401 未授權
                }
                else if (result is BadRequestObjectResult)
                {
                    return BadRequest(); // 返回 400 錯誤
                }
            }

            //組成訂單編號
            string currentTimeString = DateTime.Now.ToString("yyyyMMddHHmmss");
            string orderid = fUserId.ToString() + currentTimeString;

            //取快取的token
            //await _jwtService.GetTokenForCurrentUser();
            
            if (_memoryCache.TryGetValue(fUserId, out string token)) 
            {
                //將fUserId存入Dictionary
                OrderToken[orderid] = token;
            }

            // 將 CheckOutDTO 保存到 session 或其他臨時存儲
            //HttpContext.Session.SetString(orderid, JsonConvert.SerializeObject(checkOutDto));

            //組header
            var requestUrl = "/v3/payments/request";
            var nonce = Guid.NewGuid().ToString();
            var requestPayload = new
            {
                amount = checkOutDto.amount, //訂單總額
                currency = "TWD",  //幣值
                orderId = orderid,  //訂單編號
                packages = new[]
                {
                    new {
                        id = orderid,  //一個訂單就付款一次的話這裡就是訂單編號
                        amount = checkOutDto.amount, //相當於訂單總額
                        name = "商品名稱", //訂單描述
                        products = new[]  //訂單明細
                        {
                            new { name = "商品名稱", quantity = 1, price = checkOutDto.amount }
                        }
                    }
                },
                redirectUrls = new
                {
                    confirmUrl = "https://8d60-1-160-11-201.ngrok-free.app/api/linepay/confirm",  // 支付完成後的回調 URL
                    cancelUrl = "http://localhost:4200/#/home"  // 用戶取消支付的回調 URL
                }
            };

            // 存儲 orderId 與 checkOutDto //將 CheckOutDTO 存入全局的 Dictionary 中
            OrderPayments[orderid] = checkOutDto;

            

            var jsonPayload = JsonConvert.SerializeObject(requestPayload);
            var signature = SignatureProvider.HMACSHA256(channelSecretKey, channelSecretKey + requestUrl + jsonPayload + nonce);

            var request = new HttpRequestMessage(HttpMethod.Post, linePayBaseApiUrl + requestUrl)
            {
                Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json")
            };
            request.Headers.Add("X-LINE-ChannelId", channelId);
            request.Headers.Add("X-LINE-Authorization-Nonce", nonce);
            request.Headers.Add("X-LINE-Authorization", signature);

            var response = await _httpClient.SendAsync(request);
            var responseData = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var linePayResponse = JsonConvert.DeserializeObject<PaymentResponseDto>(responseData);

                Console.WriteLine($"Response: {responseData}");  // 檢查 LINE Pay 回應內容
                return Ok(new { paymentUrl = linePayResponse.info.paymentUrl.web });
            }
            else
            {
            // 打印出錯誤訊息以幫助調試
            Console.WriteLine($"Error: {responseData}");

            return BadRequest("Payment request failed");
            }
        }

        [HttpGet("confirm")]
        public async Task<IActionResult> ConfirmPayment([FromQuery] string transactionId, [FromQuery] string orderId)
        {
            var nonce = Guid.NewGuid().ToString();
            var apiUrl = $"/v3/payments/{transactionId}/confirm";

            // 根據 orderId 從全局的 Dictionary 中查找 CheckOutDTO
            if (!OrderPayments.TryGetValue(orderId, out var checkOutDto))
            {
                return BadRequest("Invalid orderId or no checkOutDto found.");
            }

            // 從 session 中取出 CheckOutDTO
            //var checkoutDataJson = HttpContext.Session.GetString(orderId);
            //if (string.IsNullOrEmpty(checkoutDataJson))
            //{
            //    return BadRequest("No checkout data found");
            //}

            //var checkoutDto = JsonConvert.DeserializeObject<CheckOutDTO>(checkoutDataJson);

            //確認金額
            var confirmRequest = new
            {
                amount = checkOutDto.amount,  // 金額應與支付請求中的一致
                currency = "TWD"
            };

            var jsonPayload = JsonConvert.SerializeObject(confirmRequest);
            var signatureString = channelSecretKey + apiUrl + jsonPayload + nonce;
            var signature = SignatureProvider.HMACSHA256(channelSecretKey, signatureString);
            var request = new HttpRequestMessage(HttpMethod.Post, linePayBaseApiUrl + apiUrl)
            {
                Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json")
            };
            request.Headers.Add("X-LINE-ChannelId", channelId);
            request.Headers.Add("X-LINE-Authorization-Nonce", nonce);
            request.Headers.Add("X-LINE-Authorization", signature); // 這裡的 Authorization Header 可能需要調整

            var response = await _httpClient.SendAsync(request);
            var responseData = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                // 支付成功，創建訂單
                // 根據 orderId 從全局的 Dictionary 中查找 CheckOutDTO
                if (!OrderToken.TryGetValue(orderId, out var token))
                {
                    return BadRequest("Invalid orderId or no token found.");
                }
                await CreateOrder(checkOutDto,token);
                /*Console.WriteLine($"Response: {responseData}");*/  // 檢查 LINE Pay 回應內容
                // 支付確認成功後，從字典中移除訂單資料
                OrderPayments.Remove(orderId);
                OrderToken.Remove(orderId);
                return Redirect("http://localhost:4200/#/home");
            }
            else
            {
                Console.WriteLine($"Error: {responseData}");
                return BadRequest($"Payment confirmation failed: {responseData}");
            }
        }
        private async Task CreateOrder(CheckOutDTO dto,string token)
        {
            var orderRequest = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7187/api/orders")
            {
                Content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json")
            };
            // 附加 Authorization header (JWT Token)
            orderRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var response = await _httpClient.SendAsync(orderRequest);

                // 確認 response 是否成功
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Order created successfully.");
                }
                else
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Failed to create order. Status Code: {response.StatusCode}, Response: {responseBody}");
                }
            }
            catch (Exception ex)
            {
                // 捕捉和記錄異常
                Console.WriteLine($"Error occurred while creating order: {ex.Message}");
            }
        }
    }
    
}
