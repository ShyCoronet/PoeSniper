using System;
using PoeSniperCore.EventsArgs;
using PoeSniperCore.Models;
using System.Threading.Tasks;
using Utils;
using System.Collections.Generic;
using Serilog;

namespace PoeSniperCore
{
    public class PoeTradeSniper : IDisposable
    {
        private ITradeObserver observer;
        private ILogger logger;
        private bool isActive;
        private bool isLoading;
        private bool isConnected;

        public Guid Guid { get; }
        public string Url { get; set; } = string.Empty;
        public SniperOption options { get; set; }
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
        public event EventHandler<TradeOffersEventArgs> TradeOfferMessageReceived;

        public PoeTradeSniper()
        {
            Guid = Guid.NewGuid();
            logger = new LoggerConfiguration().
                WriteTo.File(@"C:\Users\mrche\AppData\Roaming\PoeSniper\Logs\log.txt")
                .CreateLogger();
        }

        public PoeTradeSniper(SniperOption options) : this() {
            this.options = options;
        }

        public async Task TryConnect()
        {
            if (IsActive) StopSnipe();

            observer = TradeObserverFactory.CreateObserver(Url, options);

            if (observer == null) return;

            IsConnected = false;
            IsLoading = true;

            bool result = await observer.TryConnectToTradeAsync(Url);

            IsLoading = false;
            IsConnected = result;
        }

        public async Task StartSnipe()
        {
            if (!isConnected) return;

            if (IsActive) StopSnipe();

            IsActive = true;

            await observer.StartObserveAsync(Url, (result) => {
                switch (result) {
                    case Success<IEnumerable<TradeOffer>> res:
                        OnTradeOffersReceived(res.Value);
                        break;

                    case Failure<IEnumerable<TradeOffer>> res:
                        IsActive = false;
                        logger.Error(res.Exception.Message);
                        break;
                }
            });
        }

        public void StopSnipe()
        {
            if (IsActive)
            {
                observer?.StopObserve();
                IsActive = false;
            }
        }

        public void Dispose()
        {
            observer?.Dispose();
        }

        private void OnTradeOffersReceived(IEnumerable<TradeOffer> offers)
        {
            TradeOfferMessageReceived?.Invoke(this, new TradeOffersEventArgs(offers));
        }
    }
}
