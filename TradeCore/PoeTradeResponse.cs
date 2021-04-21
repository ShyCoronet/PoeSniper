using Newtonsoft.Json;
using System.Collections.Generic;

namespace PoeTrade {
    public class PoeTradeResponse {
        [JsonProperty("count")] 
        public int Count { get; set; }

        [JsonProperty("data")] 
        public string Data { get; set; }

        [JsonProperty("newid")] 
        public string NewId { get; set; }

        [JsonProperty("uniqs")] 
        public IEnumerable<string> Uniqs { get; set; }
    }
}
