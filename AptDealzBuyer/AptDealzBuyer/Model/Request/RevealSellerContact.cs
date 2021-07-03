using Newtonsoft.Json;

namespace AptDealzBuyer.Model.Request
{
    public class RevealSellerContact
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("sellerId")]
        public string SellerId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }
    }
}
