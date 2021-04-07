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

        [JsonIgnore]
        private NotificationService notificationService;

        [JsonIgnore]
        private PasteCommandHandler pasteHandler;

        private RelayCommand addSearch;
        private RelayCommand removeSearch;
        private RelayCommand removeAllSearches;
        private RelayCommand connect;
        private RelayCommand connectAllSearches;
        private RelayCommand changeStateSearch;
        private RelayCommand removeAllResutls;

        public IList<SniperViewModel> searches { get; }

        [JsonIgnore]
        public IList<string> searchResults { get; }

        public int SearchesCount
        {
            get => this.searchesCount;
            set => this.SetProperty(ref searchesCount, value, nameof(SearchesCount));
        }

        [JsonIgnore]
        public RelayCommand AddSearch
        {
            get
            {
                return addSearch ?? (addSearch = new RelayCommand(() =>
                {
                    searches.Add(new SniperViewModel());
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
                    Task.Run(() => search.TradeSniper.LoadPage());
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
                        Task.Run(() => search.TradeSniper.LoadPage());
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
                        Task.Run(() =>
                        {
                            search.TradeSniper.TradeOfferMessageReceived += OnTradeOfferReceived;
                            search.TradeSniper.StartSnipe();
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
            searches = new ObservableCollection<SniperViewModel>();
            searchResults = new ObservableCollection<string>();
            notificationService = new NotificationService();
            pasteHandler = new PasteCommandHandler();
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

        private void OnTradeOfferReceived(object sender, TradeOfferMessageEventArgs e)
        {
            var sniper = sender as PoeTradeSniper;
            var search = searches.FirstOrDefault(search => search.TradeSniper.Guid == sniper.Guid);
            notificationService.ShowNotification(search.Name, e.TradeOfferMessage);
            Application.Current.Dispatcher.Invoke(() => searchResults.Add(e.TradeOfferMessage));
        }
    }
}