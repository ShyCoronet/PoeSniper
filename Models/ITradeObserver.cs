using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utils;

namespace Models {
    public interface ITradeObserver : IDisposable {
        public Task<bool> TryConnectToTrade(string url);
        public Task TryStartObserve(string url, Action<Maybe<IEnumerable<TradeOffer>>> callback);
        public void StopObserve();
    }
}
