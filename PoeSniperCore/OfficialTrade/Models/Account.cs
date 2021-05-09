using Newtonsoft.Json;

namespace PoeSniperCore.OfficialTrade.Models {
    public class Account {
        [JsonProperty("lastCharacterName")]
        public string LastCharacterName { get; set; }
    }
}
