using PoeSniperCore;

namespace PoeSniperUI
{
    public class SniperViewModel : ViewModelBase
    {
        private string name = string.Empty;
        private bool isActive;
        private bool isLoading;
        private bool isConnected;

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

        public bool IsConnected
        {
            get => this.isConnected;
            set => this.SetProperty(ref isConnected, value, nameof(IsConnected));
        }

        public SniperViewModel(string sessionId)
        {
            TradeSniper = new PoeTradeSniper("https://www.pathofexile.com/trade/search/Ritual/1EQ0s5/live");
            TradeSniper.AuthenticationToPoeTrade(sessionId);
            TradeSniper.SniperStateChanged += OnObserverStateChanged;
            TradeSniper.LoadingStateChanged += OnLoadingStateChanged;
            TradeSniper.ConnectionStateChanged += OnConnectionStateChanged;
        }

        private void OnObserverStateChanged(object sender, SniperStateChangedEventArgs e)
        {
            IsActive = e.IsActive;
        }

        private void OnLoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            IsLoading = e.IsLoading;
        }

        private void OnConnectionStateChanged(object sender, ConnectionStateChangedEventArgs e)
        {
            IsConnected = e.IsConnected;
        }
    }
}