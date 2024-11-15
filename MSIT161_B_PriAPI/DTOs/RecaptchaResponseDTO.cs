using Newtonsoft.Json;

namespace MSIT161_B_PriAPI.DTOs
{
    public class RecaptchaResponseDTO
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("score")]
        public float Score { get; set; }

        [JsonProperty("error-codes")]
        public IEnumerable<string> ErrorCodes { get; set; }
    }
}
