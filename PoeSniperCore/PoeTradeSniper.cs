using CefSharp;
using ChromuimBrowser;
using System;
using System.Text.RegularExpressions;
using PoeSniperCore.EventsArgs;

namespace PoeSniperCore
{
    public class PoeTradeSniper : IDisposable
    {
        private OffscreenBrowser browser;
        private bool isInitialScript;
        private bool isActive;
        private bool isLoading;
        private bool isConnected;
        private Regex filter = new(@"@\S*");

        public Guid Guid { get; }
        public string Url { get; set; }
        public bool IsActive
        {
            get => isActive;
            private set
            {
                isActive = value;
                SniperStateChanged?.Invoke(this, new SniperStateChangedEventArgs(value));
            }
        }
        public bool IsLoading
        {
            get => isLoading;
            private set
            {
                isLoading = value;
                LoadingStateChanged?.Invoke(this, new EventsArgs.LoadingStateChangedEventArgs(value));
            }
        }
        public bool IsConnected
        {
            get => isConnected;
            private set
            {
                isConnected = value;
                ConnectionStateChanged?.Invoke(this, new ConnectionStateChangedEventArgs(value));
            }
        }

        public event EventHandler<SniperStateChangedEventArgs> SniperStateChanged;
        public event EventHandler<EventsArgs.LoadingStateChangedEventArgs> LoadingStateChanged;
        public event EventHandler<ConnectionStateChangedEventArgs> ConnectionStateChanged;
        public event EventHandler<TradeOfferMessageEventArgs> TradeOfferMessageReceived;

        public PoeTradeSniper()
        {
            Guid = Guid.NewGuid();
            browser = new OffscreenBrowser();
            browser.ConsoleMessageReceive += FilteringConsoleMessage;
        }

        public void LoadPage()
        {
            if (IsActive) StopSnipe();

            IsConnected = false;
            isInitialScript = false;
            IsLoading = true;
            
            browser.LoadPage(Url).Wait();

            IsLoading = false;
            IsConnected = CheckPoeTradePage();
        }

        public bool StartSnipe()
        {
            if (!isInitialScript) InitialScript();

            if (!isConnected) return false;

            string script = "observer.observe(target, config)";

            var result = browser.ExecuteJavaScriptAsync(script).Result;
            IsActive = result.Success;

            return IsActive;
        }

        public void StopSnipe()
        {
            string script = "observer.disconnect()";

            if (IsActive)
            {
                var result = browser.ExecuteJavaScriptAsync(script).Result;
                IsActive = !result.Success;
            }
        }

        public void Dispose()
        {
            browser.Dispose();
        }

        private void InitialScript()
        {
            string script = "const config = { childList: true }\n" +
                            "const callback = (mutationList, observer) => { " +
                            "const whisperBtn = document.querySelector('a.whisper-btn')\n" +
                            "if(whisperBtn !== null) { console.log(whisperMessage(whisperBtn)) } }\n" +
                            "const target = document.querySelector('#items')\n" +
                            "const observer = new MutationObserver(callback)\n";

            if (isConnected)
                isInitialScript = browser.ExecuteJavaScriptAsync(script).Result.Success;
        }

        private bool CheckPoeTradePage()
        {
            string script = "const checkingScript = () => { let result = document.querySelector('div.alert-box.live-search')\n" +
                                                            "if (result === null) throw 'Not live search' }\n" +
                                                            "checkingScript()";
            bool isPoeTradeLiveSearch = browser.ExecuteJavaScriptAsync(script).Result.Success;

            return isPoeTradeLiveSearch;
        }

        private void FilteringConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
            if (filter.IsMatch(e.Message))
                TradeOfferMessageReceived?.Invoke(this, new TradeOfferMessageEventArgs(e.Message));
        }
    }
}
