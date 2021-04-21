using Models;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Utils;

namespace PoeTrade {
    public class PoeTradeObserver : ITradeObserver {
        private ClientWebSocket webSocket;
        private ArraySegment<byte> buffer;

        private PoeTradeClient tradeClient;
        private PoeTradeParser tradeParser;

        private const string webSocketAddress = "ws://live.poe.trade/";
        private Regex regex = new Regex(@"\D*\/", RegexOptions.Compiled);
        

        public PoeTradeObserver() {
            webSocket = new ClientWebSocket();
            tradeClient = new PoeTradeClient();
            tradeParser = new PoeTradeParser();
            buffer = WebSocket.CreateClientBuffer(64, 64);
        }

        public async Task<bool> TryConnectToTrade(string url) {
            string fullSocketAddress = webSocketAddress + GetSearchId(url);

            if (webSocket.State == WebSocketState.Aborted)
                webSocket = new ClientWebSocket();

            await webSocket.ConnectAsync(new Uri(fullSocketAddress), CancellationToken.None);

            if (webSocket.State == WebSocketState.Open) return true;

            return false;
        }

        public async Task TryStartObserve(string url, Action<Maybe<IEnumerable<TradeOffer>>> callback) {
            string id = string.Empty;
            WebSocketReceiveResult result;

            if (webSocket.State == WebSocketState.Aborted)
                await TryConnectToTrade(url);

            try {
                while (webSocket.State == WebSocketState.Open) {
                    do {
                        result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);
                    }
                    while (!result.EndOfMessage);

                    if (id == string.Empty)
                        id = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, result.Count);

                    var response = await tradeClient.PostIdToTradeAsync(id, url);
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
            webSocket.Abort();
        }

        private string GetSearchId(string url) {
            return regex.Replace(url, string.Empty);
        }
    }
}