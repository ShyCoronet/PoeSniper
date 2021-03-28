using PoeSniperCore;

namespace PoeSniperUI
{
    public class SniperViewModel : ViewModelBase
    {
        private string name;
        private bool isActive;
        private bool isLoading;

        public PoeTradeSniper TradeSniper { get; }

        public string Name
        {
            get => this.name;
            set => this.name = value.Trim();
        }

        public string Url
        {
            get => this.TradeSniper.Url;
            set => this.TradeSniper.Url = value.Trim();
        }

        public bool IsActive
        {
            get => this.isActive;
            set => this.SetProperty(ref isActive, value, nameof(IsActive));
        }

        public bool IsLoading
        {
            get => this.isLoading;
            set => this.SetProperty(ref isLoading, value, nameof(IsLoading));
        }

        public SniperViewModel(string sessionId)
        {
            TradeSniper = new PoeTradeSniper("https://www.pathofexile.com/trade/search/Ritual/L5DZUn/live");
            TradeSniper.AuthenticationToPoeTrade(sessionId);
            TradeSniper.ObserverStateChanged += OnObserverStateChanged;
            TradeSniper.LoadingStateChanged += OnLoadingStateChanged;
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