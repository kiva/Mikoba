using Newtonsoft.Json;

namespace mikoba.Services
{
    public class KycResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}