using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace PoeSniperUI
{
    public class ApplicationViewModel : ViewModelBase
    {
        private const int maxSearchCount = 12;    // the maximum number of searches for an official trade is 12
        private int searchesCount;
        private string sessionId = "a140bcc59bb595c5c5253b2d091a9298";

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
                    Task.Run(() => search.TradeSniper.LoadRessource());
                },
                (search) =>
                {
                    return !search.IsLoading;
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
                            Task.Run(() => search.TradeSniper.LoadRessource());
                    }
                }));
            }
        }

        public RelayCommand ChangeStateSearch
        {
            get
            {
                return changeStateSearch ?? (changeStateSearch = new RelayCommand<SniperViewModel>(async (search) =>
                {
                    if (!search.IsActive)
                    {
                        await Task.Run(() =>
                        {
                            search.TradeSniper.TradeOfferReceived += OnTradeOfferReceived;
                            search.TradeSniper.StartSnipe();
                        });
                    }
                    else
                    {
                        await Task.Run(() =>
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
        }

        private void OnTradeOfferReceived(object sender, CefSharp.ConsoleMessageEventArgs e)
        {
            string tradeOfferMessage = Clipboard.GetText();
            object lockObj = new();
            lock (lockObj)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    searchResults.Add(tradeOfferMessage);
                });
            } 
        }
    }
}