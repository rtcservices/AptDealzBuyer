using Newtonsoft.Json;

namespace AptDealzBuyer.Model.Request
{
    public class AuthenticateEmail
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("otp")]
        public string Otp { get; set; }
    }
}
