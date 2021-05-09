using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utils;

namespace PoeSniperCore.Models {
    /// <summary>
    /// Interface that describes the behavior of listeners of trading platforms
    /// </summary>
    public interface ITradeObserver : IDisposable {
        /// <summary>
        /// Method for establishing a connection to a trading platform
        /// </summary>
        /// <param name="address"></param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// if true is returned, the connection is established
        /// </returns>
        public Task<bool> TryConnectToTradeAsync(string address);

        /// <summary>
        /// Starts listening to the trading platform for new offers and invoke a callback
        /// </summary>
        /// <param name="address"></param>
        /// <param name="callback"></param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public Task StartObserveAsync(string address, Action<Maybe<IEnumerable<TradeOffer>>> callback);

        /// <summary>
        /// Stops listening to the trading platform
        /// </summary>
        public void StopObserve();
    }
}
