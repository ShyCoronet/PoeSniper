using PoeSniperCore;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PoeSniperUI
{
    public class LiveSearch : INotifyPropertyChanged
    {
        public PoeTradeSniper TradeObserver { get; }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value.Trim();
                NotifyPropertyChanged("Name");
            }
        }

        public string Url
        {
            get { return TradeObserver.Url; }
            set
            {
                TradeObserver.Url = value.Trim();
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

        public event PropertyChangedEventHandler PropertyChanged;

        public LiveSearch(string sessionId)
        {
            TradeObserver = new PoeTradeSniper("https://www.pathofexile.com/trade/search/Ritual/L5DZUn/live");
            TradeObserver.AuthenticationToPoeTrade(sessionId);
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