using Newtonsoft.Json;

namespace mikoba
{
    public class Credential
    {
        [JsonProperty("brithDate")]
        public string BirthData { get; set; }
        
        [JsonProperty("fatherFirstName")]
        public string FatherFirstName { get; set; }

        [JsonProperty("fatherLastName")]
        public string FatherLastName { get; set; }        
        
        [JsonProperty("firstName")]
        public string FirstName { get; set; }        
        
        [JsonProperty("lastName")]
        public string LastName { get; set; }
        
        [JsonProperty("nationalId")]
        public string NationalId { get; set; }
        
        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }
        
        [JsonProperty("occupation")]
        public string Occupation { get; set; }
    }
}
