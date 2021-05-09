using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoeSniperCore.OfficialTrade.Models {

    /// <summary>
    /// Describes the headers for sending a request to https://www.pathofexile.com/api/trade/fetch/.
    /// </summary>
    public class RequestOptions {
        public string Cookie { get; }
        public string Origin { get; } = "https://www.pathofexile.com";
        public string UserAgent { get; } = "PoeSniper";
        public RequestOptions(string sessionId) {
            Cookie = $"POESESSID={sessionId}";
        }
    }
}
