using PoeSniperCore;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PoeSniperUI
{
    public class LiveSearch : INotifyPropertyChanged
    {
        public string Id { get; }
        public PoeTradeObserver TradeObserver { get; }

        public string Url
        {
            get { return TradeObserver.Url; }

            set
            {
                TradeObserver.Url = value;
                NotifyPropertyChanged("Url");
            }
        }

        public bool IsActive
        {
            get { return _isActive; }

            set
            {
                _isActive = value;
                NotifyPropertyChanged("IsActive");
            }
        }

        public bool IsLoading
        {
            get { return _isLoading; }

            set
            {
                _isLoading = value;
                NotifyPropertyChanged("IsLoading");
            }
        }

        private bool _isActive;
        private bool _isLoading;
        private Guid _guid;

        public event PropertyChangedEventHandler PropertyChanged;

        public LiveSearch()
        {
            _guid = Guid.NewGuid();
            Id = _guid.ToString();
            TradeObserver = new PoeTradeObserver("https://www.pathofexile.com/trade/search/Ritual/L5DZUn/live");
            TradeObserver.AuthenticationToPoeTrade("a140bcc59bb595c5c5253b2d091a9298");
            TradeObserver.ObserverStateChanged += OnObserverStateChanged;
            TradeObserver.LoadingStateChanged += OnLoadingStateChanged;
        }

        public void NotifyPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private void OnObserverStateChanged(object sender, ObserverStateChangedEventArgs e)
        {
            IsActive = e.IsActive;
        }

        private void OnLoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            IsLoading = e.IsLoading;
        }
    }
}
