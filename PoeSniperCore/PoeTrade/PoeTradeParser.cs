using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Html.Parser;
using PoeSniperCore.Models;

namespace PoeSniperCore.PoeTrade {
    /// <summary>
    /// Provides methods for converting responses from www.poe.trade.
    /// </summary>
    public class PoeTradeParser {
        private HtmlParser htmlParser = new HtmlParser();

        private string characterNameAttribute = "data-ign";
        private string buyoutAttribute = "data-buyout";
        private string leagueAttribute = "data-league";
        private string itemNameAttribute = "data-name";
        private string stashTabNameAttribute = "data-tab";
        private string itemPositionXAttribute = "data-x";
        private string itemPositionYAttribute = "data-y";

        /// <summary>
        /// Parses data about trade offers from html.
        /// </summary>
        /// <param name="html"></param>
        /// <exception cref="Exception"></exception>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<IEnumerable<TradeOffer>> Parse(string html) {
            try {
                var document = await htmlParser.ParseDocumentAsync(html);
                var elements = document.QuerySelectorAll("tbody.item");

                var offers = elements.Select(e => new TradeOffer(
                    e.GetAttribute(characterNameAttribute),
                    e.GetAttribute(itemNameAttribute),
                    e.GetAttribute(buyoutAttribute),
                    e.GetAttribute(leagueAttribute),
                    e.GetAttribute(stashTabNameAttribute),
                    e.GetAttribute(itemPositionXAttribute),
                    e.GetAttribute(itemPositionYAttribute)));

                return offers;
            }
            
            catch(Exception e) {
                throw new Exception(e.Message);
            }
        }
    }
}
