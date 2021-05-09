using Newtonsoft.Json;
using PoeSniperCore;
using PoeSniperCore.EventsArgs;
using System;

namespace PoeSniperUI
{
    [Serializable]
    public class SniperViewModel : ViewModelBase
    {
        private string name = string.Empty;

        private bool isActive;
        private bool isLoading;
        private bool isConnected;

        [JsonIgnore]
        public PoeTradeSniper TradeSniper { get; } = new PoeTradeSniper();

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

        [JsonIgnore]
        public bool IsActive
        {
            get => this.isActive;
            set => this.SetProperty(ref isActive, value, nameof(IsActive));
        }

        [JsonIgnore]
        public bool IsLoading
        {
            get => this.isLoading;
            set => this.SetProperty(ref isLoading, value, nameof(IsLoading));
        }

        [JsonIgnore]
        public bool IsConnected
        {
            get => this.isConnected;
            set => this.SetProperty(ref isConnected, value, nameof(IsConnected));
        }

        public SniperViewModel()
        {
            TradeSniper.SniperStateChanged += OnObserverStateChanged;
            TradeSniper.LoadingStateChanged += OnLoadingStateChanged;
            TradeSniper.ConnectionStateChanged += OnConnectionStateChanged;
        }

        public SniperViewModel(SniperOption options) : this() {
            TradeSniper.options = options;
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