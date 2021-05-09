using Newtonsoft.Json;

namespace PoeSniperCore.OfficialTrade.Models {
    public class Stash {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("x")]
        public string ItemPositionX { get; set; }

        [JsonProperty("y")]
        public string ItemPositionY { get; set; }
    }
}
