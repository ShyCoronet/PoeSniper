using PoeSniperCore.Models;
using PoeSniperCore.PoeTrade.Models;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utils;

namespace PoeSniperCore.PoeTrade {
    /// <summary>
    /// Provides methods for listening to live search on www.poe.trade.
    /// </summary>
    public class PoeTradeObserver : ITradeObserver {
        private ClientWebSocket webSocket = new ClientWebSocket();
        private ArraySegment<byte> buffer = 
            WebSocket.CreateClientBuffer(64, 64);

        private PoeTradeClient tradeClient = new PoeTradeClient();
        private PoeTradeParser tradeParser = new PoeTradeParser();
        private PoeTradeSearchInfo searchInfo;

        private const string WEBSOCKETADDRESS = "ws://live.poe.trade/";
        

        public PoeTradeObserver(string url) {
            searchInfo = new PoeTradeSearchInfo(url);
        }

        public async Task<bool> TryConnectToTradeAsync(string url) {
            string fullSocketAddress = WEBSOCKETADDRESS + searchInfo.SearchId;

            if (webSocket.State == WebSocketState.Aborted)
                webSocket = new ClientWebSocket();

            await webSocket.ConnectAsync(new Uri(fullSocketAddress), CancellationToken.None);

            if (webSocket.State == WebSocketState.Open) return true;

            return false;
        }

        public async Task StartObserveAsync(string url, Action<Maybe<IEnumerable<TradeOffer>>> callback) {
            string id = string.Empty;
            WebSocketReceiveResult result;

            if (webSocket.State == WebSocketState.Aborted)
                await TryConnectToTradeAsync(url);

            try {
                while (webSocket.State == WebSocketState.Open) {
                    do {
                        result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);
                    }
                    while (!result.EndOfMessage);

                    if (id == string.Empty)
                        id = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, result.Count);

                    var response = await tradeClient.PostRequestAsync(id, url);

                    id = response.NewId;

                    if (response.Count != 0) {
                        var offers = await tradeParser.Parse(response.Data);

                        callback?.Invoke(Maybe<IEnumerable<TradeOffer>>.TryCreateSuccess(offers));
                    }
                }
            } catch(Exception ex) {
                callback?.Invoke(Maybe<IEnumerable<TradeOffer>>.CreateFailure(ex));
            }
        }

        public void StopObserve() {
            if (webSocket.State == WebSocketState.Open)
                webSocket.Abort();
        }

        public void Dispose() {
            webSocket.Dispose();
        }
    }
}