using System;
using PoeSniperCore.EventsArgs;
using Models;
using PoeTrade;
using System.Threading.Tasks;
using Utils;
using System.Collections.Generic;

namespace PoeSniperCore
{
    public class PoeTradeSniper : IDisposable
    {
        private readonly ITradeObserver observer;
        private bool isActive;
        private bool isLoading;
        private bool isConnected;

        public Guid Guid { get; }
        public string Url { get; set; } = string.Empty;
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
        public event EventHandler<TradeOffersEventArgs> TradeOfferMessageReceived;

        public PoeTradeSniper()
        {
            Guid = Guid.NewGuid();
            observer = new PoeTradeObserver();
        }

        public async Task TryConnect()
        {
            if (IsActive) StopSnipe();

            IsConnected = false;
            IsLoading = true;

            bool result = await observer.TryConnectToTrade(Url);

            IsLoading = false;
            IsConnected = result;
        }

        public async Task StartSnipe()
        {
            if (IsActive) StopSnipe();

            if (!isConnected) return;

            IsActive = true;
            await observer.TryStartObserve(Url, (result) => {
                switch (result) {
                    case Success<IEnumerable<TradeOffer>> res:
                        OnTradeOffersReceived(res.Value);
                        break;
                    case Failure<IEnumerable<TradeOffer>> res:
                        IsActive = false;
                        throw res.Exception;
                }
            });
        }

        public void StopSnipe()
        {
            if (IsActive)
            {
                observer.StopObserve();
                IsActive = false;
            }
        }

        public void Dispose()
        {
            observer.Dispose();
        }

        private void OnTradeOffersReceived(IEnumerable<TradeOffer> offers)
        {
            TradeOfferMessageReceived?.Invoke(this, new TradeOffersEventArgs(offers));
        }
    }
}
