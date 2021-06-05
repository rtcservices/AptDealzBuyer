using Newtonsoft.Json;

namespace AptDealzBuyer.Model.Request
{
    public class Register
    {
        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }
    }

}
