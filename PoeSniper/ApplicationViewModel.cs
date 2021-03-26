using CefSharp;
using PoeSniper;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

namespace PoeSniperUI
{
    public class ApplicationViewModel : INotifyPropertyChanged

    {
        public IList<LiveSearch> searches { get; }
        public IList<string> searchResults { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        private const int _maxSearchCount = 12;    // the maximum number of searches for an official trade is 12
        private int _searchesCount;

        public int SearchesCount
        {
            get { return _searchesCount; }
            set
            {
                _searchesCount = value;
                NotifyPropertyChanged("SearchesCount");
            }
        }

        public void NotifyPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private RelayCommand _addSearch;
        public RelayCommand AddSearch
        {
            get
            {
                return _addSearch ?? (_addSearch = new RelayCommand(() =>
                {
                    searches.Add(new LiveSearch());
                    SearchesCount = searches.Count;
                }, 
                
                () => 
                {
                    return searches.Count != _maxSearchCount;
                }));
            }
        }

        private RelayCommand _removeSearch;
        public RelayCommand RemoveSearch
        {
            get
            {
                return _removeSearch ?? (_removeSearch = new RelayCommand<LiveSearch>((search) => 
                {
                    searches.Remove(search);
                    SearchesCount = searches.Count;
                    search.TradeObserver.Dispose();
                }));
            }
        }

        private RelayCommand _removeAllSearches;
        public RelayCommand RemoveAllSearches
        {
            get
            {
                return _removeAllSearches ?? (_removeAllSearches = new RelayCommand(() =>
                {
                    foreach (var search in searches)
                    {
                        search.TradeObserver.Dispose();
                    }
                    searches.Clear();
                    SearchesCount = searches.Count;
                }));
            }
        }

        private RelayCommand _connect;
        public RelayCommand Connect
        {
            get
            {
                return _connect ?? (_connect = new RelayCommand<LiveSearch>((search) =>
                {
                    Task.Run(() => search.TradeObserver.LoadRessource());
                },
                (search) =>
                {
                    return !search.IsLoading;
                }));
            }
        }

        private RelayCommand _connectAllSearches;
        public RelayCommand ConnectAllSearches
        {
            get
            {
                return _connectAllSearches ?? (_connectAllSearches = new RelayCommand(() =>
                {
                    foreach (var search in searches)
                    {
                        if (!search.IsLoading)
                            Task.Run(() => search.TradeObserver.LoadRessource());
                    }
                }));
            }
        }

        private RelayCommand _changeStateSearch;
        public RelayCommand ChangeStateSearch
        {
            get
            {
                return _changeStateSearch ?? (_changeStateSearch = new RelayCommand<LiveSearch>(async (search) =>
                {
                    if (!search.IsActive)
                    {
                        await Task.Run(() =>
                        {
                            search.TradeObserver.TradeOfferReceived += OnTradeOfferReceived;
                            search.TradeObserver.StartObserve();
                        });
                    }
                    else
                    {
                        await Task.Run(() =>
                        {
                            search.TradeObserver.TradeOfferReceived -= OnTradeOfferReceived;
                            search.TradeObserver.StopObserve();
                        });
                    }
                }));
            }
        }

        private RelayCommand _removeAllResutls;
        public RelayCommand RemoveAllResults
        {
            get
            {
                return _removeAllResutls ?? (_removeAllResutls = new RelayCommand(() =>
                {
                    searchResults.Clear();
                }));
            }
        }

        public ApplicationViewModel()
        {
            searches = new ObservableCollection<LiveSearch>();
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
