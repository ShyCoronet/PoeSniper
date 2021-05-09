using PoeSniperCore.Models;
using PoeSniperCore.OfficialTrade.Models;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using Utils;

namespace PoeSniperCore.OfficialTrade {

    /// <summary>
    /// Provides methods for listening to live search on www.pathofexile.com/trade.
    /// </summary>
    public class OfficialTradeObserver : ITradeObserver {
        private ClientWebSocket webSocket;
        private ArraySegment<byte> buffer;
        private const string webSocketAddress = "wss://www.pathofexile.com/api/trade/live/";

        private OfficialTradeClient client;
        private OfficialTradeParser parser;
        private RequestOptions options;
        private OfficialTradeSearchInfo urlInfo;

        public OfficialTradeObserver(string url, string sessionId) {
            buffer = WebSocket.CreateClientBuffer(512, 512);
            options = new RequestOptions(sessionId);
            webSocket = CreateSocketWithHeaders();
            parser = new OfficialTradeParser();
            client = new OfficialTradeClient(options);
            urlInfo = new OfficialTradeSearchInfo(url);
        }

        public async Task<bool> TryConnectToTradeAsync(string url) {
            string fullSocketAddress = webSocketAddress + $"{urlInfo.League}/{urlInfo.SearchId}";

            if (webSocket.State == WebSocketState.Aborted)
                webSocket = new ClientWebSocket();

            await webSocket.ConnectAsync(new Uri(fullSocketAddress), CancellationToken.None);

            if (webSocket.State == WebSocketState.Open) return true;

            return false;
        }

        public async Task StartObserveAsync(string url, Action<Maybe<IEnumerable<TradeOffer>>> callback) {
            bool isAuth = false;
            WebSocketReceiveResult result;

            if (webSocket.State == WebSocketState.Aborted)
                await TryConnectToTradeAsync(url);

            try {
                while (webSocket.State == WebSocketState.Open) {
                    do {
                        result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);
                    }
                    while (!result.EndOfMessage);

                    var message = parser.ParseMessage(buffer, result.Count);

                    if (!isAuth && !message.IsAuthenticated)
                        callback?.Invoke(Maybe<IEnumerable<TradeOffer>>.CreateFailure(
                            new AuthenticationException()));
                    isAuth = true;

                    if (message.OffersId != null)
                    {
                        var response = await client.GetResponseAsync(urlInfo.SearchId, message.OffersId);
                        var offers = parser.ParseResponse(response);
                        callback?.Invoke(Maybe<IEnumerable<TradeOffer>>.TryCreateSuccess(offers));
                    }
                }
            }
            catch (Exception ex) {
                callback?.Invoke(Maybe<IEnumerable<TradeOffer>>.CreateFailure(ex));
            }
        }

        public void StopObserve() {
            webSocket.Abort();
        }

        public void Dispose() {
            webSocket.Dispose();
        }

        private ClientWebSocket CreateSocketWithHeaders() {
            var socket = new ClientWebSocket();

            socket.Options.SetRequestHeader("Cookie", options.Cookie);
            socket.Options.SetRequestHeader("Origin", options.Origin);
            socket.Options.SetRequestHeader("User-Agent", options.UserAgent);

            return socket;
        }
    }
}
