using Newtonsoft.Json;
using System.Collections.Generic;

namespace PoeSniperCore.OfficialTrade.Models {
    /// <summary>
    /// Describes https response received from https://www.pathofexile.com/api/trade/fetch/.
    /// </summary>
    public class OfficialTradeResponse {
        [JsonProperty("result")]
        public List<Result> Results { get; set; }
    }
}
