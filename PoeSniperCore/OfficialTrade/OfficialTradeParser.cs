using Newtonsoft.Json;
using PoeSniperCore.Models;
using PoeSniperCore.OfficialTrade.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PoeSniperCore.OfficialTrade {
    /// <summary>
    /// Provides methods for converting responses from www.pathofexile.com/trade
    /// </summary>
    public class OfficialTradeParser {

        /// <summary>
        /// Converts a websocket message
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="count"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns>Message with live search information</returns>
        public OfficialTradeMessage ParseMessage(ArraySegment<byte> buffer, int count) {
            string message = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, count);

            if (string.IsNullOrEmpty(message))
                throw new ArgumentException($"Number of messages: {count}");

            return JsonConvert.DeserializeObject<OfficialTradeMessage>(message);
        }

        /// <summary>
        /// Converts the response to a collection of trade offers
        /// </summary>
        /// <param name="response"></param>
        /// <returns>Collection of trade offers</returns>
        public IEnumerable<TradeOffer> ParseResponse(OfficialTradeResponse response) {
            var offers = response.Results.Select(r => new TradeOffer(
                r.Listing.Account.LastCharacterName,
                r.Item.Name, 
                $"{r.Listing.Price?.Amount} {r.Listing.Price?.Currency}",
                r.Item.League,
                r.Listing.Stash?.Name,
                r.Listing.Stash?.ItemPositionX,
                r.Listing.Stash?.ItemPositionY));

            return offers;
        }
    }
}
