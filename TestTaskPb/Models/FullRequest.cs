using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TestTaskPb.Models
{
    
    public class FullRequest 
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
        [JsonPropertyName("ip")]
        public string IP { get; set; }

    }
}
