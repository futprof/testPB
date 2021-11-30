using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ClientRequestApp
{
    //This model with in client ip adress extends reqest model
    public class FullRequest: Request
    {
        [JsonPropertyName("ip")]
        public string IP { get; set; }
        public FullRequest(Request request)
        {
            Amount = request.Amount;
            ClientId = request.ClientId;
            Currency = request.Currency;
            DepartemntAddress = request.DepartemntAddress;
        }
           
    }
}
