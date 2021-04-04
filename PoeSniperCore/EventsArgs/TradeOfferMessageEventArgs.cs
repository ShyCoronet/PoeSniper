using System;

namespace PoeSniperCore.EventsArgs
{
    public class TradeOfferMessageEventArgs : EventArgs
    {
        public string TradeOfferMessage { get; }

        public TradeOfferMessageEventArgs(string message)
        {
            TradeOfferMessage = message;
        }
    }
}
