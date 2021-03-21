using PoeSniperCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

namespace PoeSniperUI
{
    public class ApplicationViewModel
    {
        public IList<LiveSearch> searches { get; }

        private const int _maxSearchCount = 12;    // the maximum number of searches for an official trade is 12

        private RelayCommand _addSearch;
        public RelayCommand AddSearch
        {
            get
            {
                return _addSearch ?? (_addSearch = new RelayCommand(() =>
                {
                    searches.Add(new LiveSearch());
                }, 
                
                () => 
                {
                    return searches.Count != _maxSearchCount;
                }));
            }
        }

        private RelayCommand _connect;
        public RelayCommand Connect
        {
            get
            {
                return _connect ?? (_connect = new RelayCommand<LiveSearch>(async (search) =>
                {
                    await Task.Run(() => search.TradeObserver.LoadRessource());
                },
                (search) =>
                {
                    return !search.IsLoading;
                }));
            }
        }

        private RelayCommand _startSearch;
        public RelayCommand StartSearch
        {
            get
            {
                return _startSearch ?? (_startSearch = new RelayCommand<LiveSearch>(async (search) =>
                {
                    if (!search.IsActive)
                    {
                        await Task.Run(() => search.TradeObserver.StartObserve());
                    }
                    else
                    {
                        await Task.Run(() => search.TradeObserver.StopObserve());
                    }
                }));
            }
        }

        public ApplicationViewModel()
        {
            searches = new ObservableCollection<LiveSearch>() { new LiveSearch() };
            searches[0].TradeObserver.TradeOfferReceived += TradeObserver_TradeOfferReceived;
        }

        private void TradeObserver_TradeOfferReceived(object sender, CefSharp.ConsoleMessageEventArgs e)
        {
            
        }
    }
}
