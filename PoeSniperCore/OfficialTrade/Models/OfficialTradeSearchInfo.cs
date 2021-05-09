namespace PoeSniperCore.OfficialTrade.Models {

    /// <summary>
    /// Contains information about live search url www.pathofexile.com/trade
    /// </summary>
    public class OfficialTradeSearchInfo {
        public string League { get; }
        public string SearchId { get; }

        public OfficialTradeSearchInfo(string url) {
            var splitedUrl = url.Split('/');

            League = splitedUrl[5];
            SearchId = splitedUrl[6];
        }
    }
}
