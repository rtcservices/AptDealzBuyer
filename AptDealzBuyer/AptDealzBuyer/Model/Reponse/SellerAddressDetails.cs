using Newtonsoft.Json;

namespace AptDealzBuyer.Model.Reponse
{
    public class SellerAddressDetails
    {
        [JsonProperty("building")]
        public string Building { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("state")]
        public object State { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("pinCode")]
        public object PinCode { get; set; }

        [JsonProperty("landmark")]
        public string Landmark { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }
    }

}
