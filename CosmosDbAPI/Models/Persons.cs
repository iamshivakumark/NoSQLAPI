using Newtonsoft.Json;

namespace CosmosDbAPI.Models
{
    public class Persons
    {
        [JsonProperty("id")]
        public string PersonID { get; set; }

        [JsonProperty("firstname")]
        public string? FirstName { get; set; }
        
        [JsonProperty("Lastname")]
        public string? LastName { get; set; }
        [JsonProperty("age")]
        public string? Age { get; set; }
        [JsonProperty("phone")]
        public string? ContactNo { get; set; }
    }
}
