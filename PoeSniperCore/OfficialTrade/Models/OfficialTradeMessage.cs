using Newtonsoft.Json;
using System.Collections.Generic;

namespace PoeSniperCore.OfficialTrade.Models {
    /// <summary>
    /// Describes the websocket message received from wss://www.pathofexile.com/api/trade/live/.
    /// </summary>
    public class OfficialTradeMessage {

        [JsonProperty("auth")]
        public bool IsAuthenticated { get; set; }

        [JsonProperty("new")]
        public IEnumerable<string> OffersId { get; set; }
    }
}
