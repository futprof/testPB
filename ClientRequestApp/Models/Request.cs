using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ClientRequestApp
{
    public class Request
    {
        [Required]
        [JsonPropertyName("client_id")]
        public string ClientId { get; set; }
        
        [Required]
        [JsonPropertyName("departemnt_address")]
        public string DepartemntAddress { get; set; }
        
        [Required]
        [JsonPropertyName("amount")]
        public double Amount { get; set; }

        [Required]
        [JsonPropertyName("currency")]
        public string Currency { get; set; }
        
    }
}
