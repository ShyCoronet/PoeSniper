using Newtonsoft.Json;

namespace PoeSniperCore.OfficialTrade.Models {
    public class Price {
        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }
    }
}
