using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using KeyboardHook;

namespace PoeSniperUI
{
    public class ApplicationViewModel : ViewModelBase
    {
        private const int maxSearchCount = 12;    // the maximum number of searches for an official trade is 12
        private int searchesCount;
        private string sessionId = "a140bcc59bb595c5c5253b2d091a9298";
        private NotificationService notificationService;
        private PasteCommandHandler pasteHandler;

        private RelayCommand addSearch;
        private RelayCommand removeSearch;
        private RelayCommand removeAllSearches;
        private RelayCommand connect;
        private RelayCommand connectAllSearches;
        private RelayCommand changeStateSearch;
        private RelayCommand removeAllResutls;

        public IList<SniperViewModel> searches { get; }
        public IList<string> searchResults { get; }

        public int SearchesCount
        {
            get => this.searchesCount;
            set => this.SetProperty(ref searchesCount, value, nameof(SearchesCount));
        }

        public string SessionId
        {
            get => this.sessionId;
            set => this.SetProperty(ref sessionId, value.Trim(), nameof(SessionId));
        }

        public RelayCommand AddSearch
        {
            get
            {
                return addSearch ?? (addSearch = new RelayCommand(() =>
                {
                    searches.Add(new SniperViewModel(SessionId));
                    SearchesCount = searches.Count;
                }, 
                
                () => 
                {
                    return searches.Count != maxSearchCount && sessionId != string.Empty;
                }));
            }
        }

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
                            search.TradeSniper.TradeOfferReceived += OnTradeOfferReceived;
                            search.TradeSniper.StartSnipe();
                        });
                    }
                    else
                    {
                        Task.Run(() =>
                        {
                            search.TradeSniper.TradeOfferReceived -= OnTradeOfferReceived;
                            search.TradeSniper.StopSnipe();
                        });
                    }
                }));
            }
        }
 
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
            pasteHandler.Install();
        }

        private void PopTradeOffer(object sender, EventArgs e)
        {
            if (searchResults.Count != 0)
            {
                Clipboard.SetText(searchResults[0]);
                searchResults.RemoveAt(0);
            }
        }

        private void OnTradeOfferReceived(object sender, CefSharp.ConsoleMessageEventArgs e)
        {
            notificationService.ShowNotification(e.Message);
            Application.Current.Dispatcher.Invoke(() => searchResults.Add(e.Message));
        }
    }
}