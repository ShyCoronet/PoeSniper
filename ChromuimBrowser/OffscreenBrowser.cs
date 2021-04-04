using CefSharp;
using CefSharp.OffScreen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ChromuimBrowser
{
    public class OffscreenBrowser : IDisposable
    {
        private ChromiumWebBrowser browser;
        private AutoResetEvent autoResetEvent;

        public event EventHandler<ConsoleMessageEventArgs> ConsoleMessageReceive
        {
            add => browser.ConsoleMessage += value;
            remove => browser.ConsoleMessage -= value;
        }

        public OffscreenBrowser()
        {
            var settings = new CefSettings();
            settings.CefCommandLineArgs["autoplay-policy"] = "no-user-gesture-required";
            
            if (!Cef.IsInitialized)
                Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);

            CefSharpSettings.ShutdownOnExit = true;

            browser = new ChromiumWebBrowser("", null, new RequestContext());
            autoResetEvent = new AutoResetEvent(false);

            browser.BrowserInitialized += InitialBrowser;
            autoResetEvent.WaitOne();
        }

        public Task LoadPage(string url)
        {
            var task = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

            EventHandler<LoadingStateChangedEventArgs> handler = null;
            handler = (sender, args) =>
            {
                if (!args.IsLoading)
                {
                    browser.LoadingStateChanged -= handler;
                    task.TrySetResult(true);
                }
            };

            browser.LoadingStateChanged += handler;

            if (!string.IsNullOrEmpty(url)) browser.Load(url);

            return task.Task;
        }

        public async Task<string> GetPageSourceAsync()
        {
            return await browser.GetSourceAsync();
        }

        public  void SetCookie(string adress, string name, string value)
        {
            var cookie = new Cookie
            {
                Name = name,
                Value = value
            };
            
            browser.GetCookieManager().SetCookie(adress, cookie);
        }

        public async Task<Dictionary<string, string>> GetCookies()
        {
            var cookieDictionary = new Dictionary<string, string>();
            var cookieList = await browser.GetCookieManager().VisitAllCookiesAsync();

            foreach (var cookie in cookieList)
            {
                cookieDictionary.Add(cookie.Name, cookie.Value);
            }

            return cookieDictionary;
        }

        public async Task<JavascriptResponse> ExecuteJavaScriptAsync(string script)
        {
            return await browser.EvaluateScriptAsync(script);
        }

        public void Dispose()
        {
            browser.Dispose();
        }

        private void InitialBrowser(object sender, EventArgs e)
        {
            if (browser.IsBrowserInitialized) autoResetEvent.Set();
        }
    }
}
