using PoeSniperCore;
using PoeSniperCore.EventsArgs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Windows;

namespace PoeSniperUI
{
    [Serializable]
    public class ApplicationViewModel : ViewModelBase
    {
        private int searchesCount;
        private string sessionId = string.Empty;

        [JsonIgnore]
        private NotificationService notificationService =
            new NotificationService();

        [JsonIgnore]
        private PasteCommandHandler pasteHandler =
            new PasteCommandHandler();

        private RelayCommand addSearch;
        private RelayCommand removeSearch;
        private RelayCommand removeAllSearches;
        private RelayCommand connect;
        private RelayCommand connectAllSearches;
        private RelayCommand changeStateSearch;
        private RelayCommand removeAllResutls;

        public IList<SniperViewModel> searches { get; } = 
            new ObservableCollection<SniperViewModel>();

        [JsonIgnore]
        public IList<string> searchResults { get; } = 
            new ObservableCollection<string>();

        public int SearchesCount
        {
            get => searchesCount;
            set => SetProperty(ref searchesCount, value, nameof(SearchesCount));
        }

        public string SessionId {
            get => sessionId;
            set => SetProperty(ref sessionId, value, nameof(SessionId));
        }

        [JsonIgnore]
        public RelayCommand AddSearch
        {
            get
            {
                return addSearch ?? (addSearch = new RelayCommand(() =>
                {
                    var options = new SniperOption(new Dictionary<string, string> { { "POESESSID", sessionId } });
                    searches.Add(new SniperViewModel(options));
                    SearchesCount = searches.Count;
                }));
            }
        }

        [JsonIgnore]
        public RelayCommand RemoveSearch
        {
            get
            {
                return removeSearch ?? (removeSearch = new RelayCommand<SniperViewModel>((search) => 
                {
                    searches.Remove(search);
                    SearchesCount = searches.Count;
                    search.TradeSniper.Dispose();
                }));
            }
        }

        [JsonIgnore]
        public RelayCommand RemoveAllSearches
        {
            get
            {
                return removeAllSearches ?? (removeAllSearches = new RelayCommand(() =>
                {
                    foreach (var search in searches)
                    {
                        search.TradeSniper.Dispose();
                    }
                    searches.Clear();
                    SearchesCount = searches.Count;
                }));
            }
        }

        [JsonIgnore]
        public RelayCommand Connect
        {
            get
            {
                return connect ?? (connect = new RelayCommand<SniperViewModel>((search) =>
                {
                    Task.Run(() => search.TradeSniper.TryConnect());
                },
                (search) =>
                {
                    return !search.IsLoading && !(search.Name == string.Empty) && 
                    !(search.Url == string.Empty) && !(search.IsActive);
                }));
            }
        }

        [JsonIgnore]
        public RelayCommand ConnectAllSearches
        {
            get
            {
                return connectAllSearches ?? (connectAllSearches = new RelayCommand(() =>
                {
                foreach (var search in searches)
                {
                    if (!search.IsLoading)
                        Task.Run(() => search.TradeSniper.TryConnect());
                }
                }));
            }
        }

        [JsonIgnore]
        public RelayCommand ChangeStateSearch
        {
            get
            {
                return changeStateSearch ?? (changeStateSearch = new RelayCommand<SniperViewModel>((search) =>
                {
                    if (!search.IsActive)
                    {
                        Task.Run(async () =>
                        {
                            search.TradeSniper.TradeOfferMessageReceived += OnTradeOfferReceived;
                            await search.TradeSniper.StartSnipe();
                        });
                    }
                    else
                    {
                        Task.Run(() =>
                        {
                            search.TradeSniper.TradeOfferMessageReceived -= OnTradeOfferReceived;
                            search.TradeSniper.StopSnipe();
                        });
                    }
                }));
            }
        }

        [JsonIgnore]
        public RelayCommand RemoveAllResults
        {
            get
            {
                return removeAllResutls ?? (removeAllResutls = new RelayCommand(() =>
                {
                    searchResults.Clear();
                }));
            }
        }

        public ApplicationViewModel()
        {
            pasteHandler.Pasted += PopTradeOffer;
            pasteHandler.Start();
        }

        private void PopTradeOffer(object sender, EventArgs e)
        {
            if (searchResults.Count != 0)
            {
                Clipboard.SetText(searchResults[0]);
                searchResults.RemoveAt(0);
            }
        }

        private void OnTradeOfferReceived(object sender, TradeOffersEventArgs e)
        {
            var sniper = sender as PoeTradeSniper;
            var search = searches.FirstOrDefault(search => search.TradeSniper.Guid == sniper.Guid);
            string message = $"{e.TradeOffers.Count()} new items have matched your search.";

            notificationService.ShowNotification(search.Name, message);

            foreach (var offer in e.TradeOffers) {
                Application.Current.Dispatcher.Invoke(() => searchResults.Add(offer.ToString()));
            }
        }
    }
}