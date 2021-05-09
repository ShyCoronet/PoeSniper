using PoeSniperCore.Models;
using PoeSniperCore.OfficialTrade;
using PoeSniperCore.PoeTrade;
using System.Text.RegularExpressions;

namespace PoeSniperCore {
    public static class TradeObserverFactory {
        private static Regex offTradePattern = 
            new("pathofexile\\.com/trade/search/", RegexOptions.Compiled);

        private static Regex poeTradePattern = 
            new("poe\\.trade/search/", RegexOptions.Compiled);

        public static ITradeObserver CreateObserver(string url, SniperOption options) {
            if (offTradePattern.IsMatch(url))
            {
                return new OfficialTradeObserver(url, options?["POESESSID"]);
            }
            
            if (poeTradePattern.IsMatch(url))
            {
                return new PoeTradeObserver(url);
            }

            return null;
        }
    }
}
