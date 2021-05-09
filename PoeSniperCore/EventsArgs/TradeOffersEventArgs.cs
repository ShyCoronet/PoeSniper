using PoeSniperCore.Models;
using System;
using System.Collections.Generic;

namespace PoeSniperCore.EventsArgs
{
    public class TradeOffersEventArgs : EventArgs
    {
        public IEnumerable<TradeOffer> TradeOffers { get; }

        public TradeOffersEventArgs(IEnumerable<TradeOffer> offers)
        {
            TradeOffers = offers;
        }
    }
}
