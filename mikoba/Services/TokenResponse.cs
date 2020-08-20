using Newtonsoft.Json;

namespace mikoba.Services
{
    public class TokenResponse
    {
        public string picture { get; set; }

        [JsonProperty("sub")] public string Id { get; set; }

        [JsonProperty("nickname")] public string Username { get; set; }

        [JsonProperty("https://ekyc.sl.kiva.org/firstName")]
        public string FirstName { get; set; }

        [JsonProperty("https://ekyc.sl.kiva.org/lastName")]
        public string LastName { get; set; }

        [JsonProperty("https://ekyc.sl.kiva.org/institution")]
        public string Institution { get; set; }

        [JsonProperty("https://ekyc.sl.kiva.org/branchLocation")]
        public string Location { get; set; }

        [JsonProperty("https://ekyc.sl.kiva.org/fspId")]
        public string FspId { get; set; }

        [JsonProperty("https://ekyc.sl.kiva.org/ekycIdChain")]
        public string EkycIdChain { get; set; }

        public string access_token { get; set; }

        public string id_token { get; set; }
    }
}
