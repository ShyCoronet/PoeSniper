﻿using CefSharp;
using ChromuimBrowser;
using System;
using System.Threading.Tasks;

namespace PoeSniperCore
{
    public class PoeTradeObserver : IDisposable
    {
        public string Url { get; set; }
        public bool IsActive { get; private set; }
        public bool IsLoading { get; private set; }

        public event EventHandler<ObserverStateChangedEventArgs> ObserverStateChanged;
        public event EventHandler<LoadingStateChangedEventArgs> LoadingStateChanged;
        public event EventHandler<ConsoleMessageEventArgs> TradeOfferReceived
        {
            add => _browser.ConsoleMessageReceive += value;
            remove => _browser.ConsoleMessageReceive -= value;
        }

        private OffscreenBrowser _browser;
        private bool _isInitialScript;

        public PoeTradeObserver(string url)
        {
            _browser = new OffscreenBrowser();
            Url = url;
        }

        public void AuthenticationToPoeTrade(string sessionId)
        {
            _browser.SetCookie("https://www.pathofexile.com/", "POESESSID", sessionId);
        }

        public void LoadRessource()
        {
            if (IsActive) StopObserve();

            IsLoading = true;
            LoadingStateChanged?.Invoke(this, new LoadingStateChangedEventArgs(IsLoading));

            _browser.LoadPage(Url).Wait();

            Task.Delay(1000).Wait(); // Required for full execution of all scripts on the page

            IsLoading = false;
            LoadingStateChanged?.Invoke(this, new LoadingStateChangedEventArgs(IsLoading));
        }

        public bool StartObserve()
        {
            if (!_isInitialScript) InitialSript();

            string script = "observer.observe(target, config)";

            var result = _browser.ExecuteJavaScriptAsync(script).Result;
            IsActive = result.Success;
            ObserverStateChanged?.Invoke(this, new ObserverStateChangedEventArgs(result.Success));

            return IsActive;
        }

        public void StopObserve()
        {
            string script = "observer.disconnect()";

            if (IsActive)
            {
                var result = _browser.ExecuteJavaScriptAsync(script).Result;
                if (result.Success) 
                {
                    IsActive = false;
                    ObserverStateChanged?.Invoke(this, new ObserverStateChangedEventArgs(false));
                };
            }           
        }

        private void InitialSript()
        {
            string script = "const config = { childList: true }\n" +
                            "const callback = (mutationList, observer) => { const whisperBtn = document.querySelector('button.btn.btn-default.whisper-btn')\nif(whisperBtn !== null) { whisperBtn.click()\nconsole.log('click') } }\n" +
                            "const target = document.querySelector('div.results')\n" +
                            "const observer = new MutationObserver(callback)\n";

            _isInitialScript = _browser.ExecuteJavaScriptAsync(script).Result.Success;
        }

        public void Dispose()
        {
            _browser.Dispose();
        }
    }
}
