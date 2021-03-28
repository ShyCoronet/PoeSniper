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

        public string Url { get; set; }
        public bool IsActive { get; private set; }
        public bool IsLoading { get; private set; }

        public event EventHandler<ObserverStateChangedEventArgs> ObserverStateChanged;
        public event EventHandler<LoadingStateChangedEventArgs> LoadingStateChanged;
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

        public void LoadRessource()
        {
            if (IsActive) StopSnipe();

            IsLoading = true;
            LoadingStateChanged?.Invoke(this, new LoadingStateChangedEventArgs(IsLoading));

            browser.LoadPage(Url).Wait();

            Task.Delay(1000).Wait(); // Required for full execution of all scripts on the page

            IsLoading = false;
            LoadingStateChanged?.Invoke(this, new LoadingStateChangedEventArgs(IsLoading));
        }

        public bool StartSnipe()
        {
            if (!isInitialScript) InitialScript();

            string script = "observer.observe(target, config)";

            var result = browser.ExecuteJavaScriptAsync(script).Result;
            IsActive = result.Success;
            ObserverStateChanged?.Invoke(this, new ObserverStateChangedEventArgs(result.Success));

            return IsActive;
        }

        public void StopSnipe()
        {
            string script = "observer.disconnect()";

            if (IsActive)
            {
                var result = browser.ExecuteJavaScriptAsync(script).Result;
                if (result.Success) 
                {
                    IsActive = false;
                    ObserverStateChanged?.Invoke(this, new ObserverStateChangedEventArgs(false));
                };
            }         
        }

        public void Dispose()
        {
            browser.Dispose();
        }

        private void InitialScript()
        {
            string script = "const config = { childList: true }\n" +
                            "const callback = (mutationList, observer) => { const whisperBtn = document.querySelector('button.btn.btn-default.whisper-btn')\nif(whisperBtn !== null) { whisperBtn.click()\nconsole.log('click') } }\n" +
                            "const target = document.querySelector('div.results')\n" +
                            "const observer = new MutationObserver(callback)\n";

            isInitialScript = browser.ExecuteJavaScriptAsync(script).Result.Success;
        }
    }
}
