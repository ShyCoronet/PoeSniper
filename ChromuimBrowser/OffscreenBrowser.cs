using CefSharp;
using CefSharp.OffScreen;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ChromuimBrowser
{
    public class OffscreenBrowser
    {
        private ChromiumWebBrowser _browser;
        private AutoResetEvent _autoResetEvent;

        public event EventHandler<ConsoleMessageEventArgs> ConsoleMessageReceive
        {
            add => _browser.ConsoleMessage += value;
            remove => _browser.ConsoleMessage -= value;
        }

        public OffscreenBrowser()
        {
            var settings = new CefSettings() { CachePath = @"D:\CefSharpExample\CefSharpExample\Cache\" };
            settings.CefCommandLineArgs["autoplay-policy"] = "no-user-gesture-required";

            try
            {
                Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);
            }
            catch { }

            CefSharpSettings.ShutdownOnExit = true;

            _browser = new ChromiumWebBrowser("", null, new RequestContext());
            _autoResetEvent = new AutoResetEvent(false);
            _browser.BrowserInitialized += InitialBrowser;
            
            _autoResetEvent.WaitOne();
        }

        public Task LoadPage(string url)
        {
            var task = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

            EventHandler<LoadingStateChangedEventArgs> handler = null;
            handler = (sender, args) =>
            {
                if (!args.IsLoading)
                {
                    _browser.LoadingStateChanged -= handler;
                    task.TrySetResult(true);
                }
            };

            _browser.LoadingStateChanged += handler;

            if (!string.IsNullOrEmpty(url))
            {
                _browser.Load(url);
            }

            return task.Task;
        }

        public async Task<string> GetPageSourceAsync()
        {
            return await _browser.GetSourceAsync();
        }

        private void InitialBrowser(object sender, EventArgs e)
        {
            if (_browser.IsBrowserInitialized)
            {
                _autoResetEvent.Set();
            }
        }

        public  void SetCookie(string adress, string name, string value)
        {
            var cookie = new Cookie
            {
                Name = name,
                Value = value
            };
            
            _browser.GetCookieManager().SetCookie(adress, cookie);
        }

        public async Task<Dictionary<string, string>> GetCookies()
        {
            var cookieDictionary = new Dictionary<string, string>();
            var cookieList = await _browser.GetCookieManager().VisitAllCookiesAsync();

            foreach (var cookie in cookieList)
            {
                cookieDictionary.Add(cookie.Name, cookie.Value);
            }

            return cookieDictionary;
        }

        public async Task<JavascriptResponse> ExecuteJavaScriptAsync(string script)
        {
            return await _browser.EvaluateScriptAsync(script);
        }
    }
}
