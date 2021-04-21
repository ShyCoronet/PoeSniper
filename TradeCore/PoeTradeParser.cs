using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Html.Parser;
using Models;

namespace PoeTrade {
    public class PoeTradeParser {
        private HtmlParser htmlParser;
        private Dictionary<string, string> attributesName
            = new Dictionary<string, string> {
                { "CharacterName", "data-ign" },
                { "Buyout", "data-buyout" },
                { "League", "data-league" },
                { "ItemName", "data-name" },
                { "StashTabName", "data-tab" },
                { "ItemPositionX", "data-x" },
                { "ItemPositionY", "data-y" }
            };

        public PoeTradeParser() {
            htmlParser = new HtmlParser();
        }
        public async Task<IEnumerable<TradeOffer>> Parse(string html) {
            try {
                var document = await htmlParser.ParseDocumentAsync(html);
                var elements = document.QuerySelectorAll("tbody.item");

                var offers = elements.Select(e => new TradeOffer(
                    e.GetAttribute(attributesName["CharacterName"]),
                    e.GetAttribute(attributesName["ItemName"]),
                    e.GetAttribute(attributesName["Buyout"]),
                    e.GetAttribute(attributesName["League"]),
                    e.GetAttribute(attributesName["StashTabName"]),
                    e.GetAttribute(attributesName["ItemPositionX"]),
                    e.GetAttribute(attributesName["ItemPositionY"])));

                return offers;
            }
            
            catch {
                throw new InvalidDataException();
            }
        }
    }
}
