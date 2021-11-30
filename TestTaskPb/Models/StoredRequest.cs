using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TestTaskPb.Models
{
    public class StoredRequest
    {
        [JsonPropertyName("amount")]
        public double Amount { get; set; }
        [JsonPropertyName("currency")]
        public string Currency { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}
