using System;
using System.Net.WebSockets;

namespace WebSocketClient {
    public class WebSocketClient {
        private ClientWebSocket client;
        private Uri uri;

        public WebSocketClient(string url, int receiveBufferSize, int sendBufferSize) {
            uri = new Uri(url);
        }
    }
}
