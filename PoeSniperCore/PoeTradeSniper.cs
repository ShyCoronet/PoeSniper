using CefSharp;
using ChromuimBrowser;
using System;
using System.Threading.Tasks;

namespace PoeSniperCore
{
    public class PoeTradeSniper : IDisposable
    {
        private OffscreenBrowser browser;
        private bool isInitialScript;
        private bool isActive;
        private bool isLoading;
        private bool isConnected;

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
                LoadingStateChanged?.Invoke(this, new LoadingStateChangedEventArgs(value));
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
        public event EventHandler<LoadingStateChangedEventArgs> LoadingStateChanged;
        public event EventHandler<ConnectionStateChangedEventArgs> ConnectionStateChanged; 
        public event EventHandler<ConsoleMessageEventArgs> TradeOfferReceived
        {
            add => browser.ConsoleMessageReceive += value;
            remove => browser.ConsoleMessageReceive -= value;
        }

        public PoeTradeSniper(string url)
        {
            browser = new OffscreenBrowser();
            Url = url;
        }

        public void AuthenticationToPoeTrade(string sessionId)
        {
            browser.SetCookie("https://www.pathofexile.com/", "POESESSID", sessionId);
        }

        public void LoadPage()
        {
            if (IsActive) StopSnipe();

            IsConnected = false;
            isInitialScript = false;
            IsLoading = true;
            
            browser.LoadPage(Url).Wait();
            Task.Delay(1000).Wait(); // Required for full execution of all scripts on the page

            IsLoading = false;
            IsConnected = CheckPoeTradePage();
        }

        public bool StartSnipe()
        {
            if (!isInitialScript) InitialScript();

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
                            "const callback = (mutationList, observer) => { const whisperBtn = document.querySelector('button.btn.btn-default.whisper-btn')\nif(whisperBtn !== null) { console.log(whisperBtn._v_clipboard.text()) } }\n" +
                            "const target = document.querySelector('div.results')\n" +
                            "const observer = new MutationObserver(callback)\n";

            isInitialScript = browser.ExecuteJavaScriptAsync(script).Result.Success;
        }

        private bool CheckPoeTradePage()
        {
            string script = "const checkingScript = () => {let result = document.querySelector('button.btn.livesearch-btn')" +
                                                            ".children[1].textContent === 'Deactivate Live Search'\n" +
                                                            "if(!result) throw 'Not live search' }\n" +
                                                            "checkingScript()";
            bool isPoeTradeLiveSearch = browser.ExecuteJavaScriptAsync(script).Result.Success;  

            return isPoeTradeLiveSearch;
        }
    }
}
