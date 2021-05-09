namespace PoeSniperCore.PoeTrade.Models {
    /// <summary>
    /// Contains information about live search url www.poe.trade.
    /// </summary>
    public class PoeTradeSearchInfo {
        public string SearchId { get; }
        public PoeTradeSearchInfo(string url) {
            var splittedUrl = url.Split('/');
            SearchId = splittedUrl[4];
        }
    }
}
