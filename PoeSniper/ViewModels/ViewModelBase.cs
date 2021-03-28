using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PoeSniperUI
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        public void NotifyPropertyChanged([CallerMemberName]string property="")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        protected bool SetProperty<T>(ref T storage, T value, string propertyName)
        {
            if (CompareValues<T>(storage, value)) return false;
            storage = value;
            NotifyPropertyChanged(propertyName);
            return true;
        }

        private bool CompareValues<T>(T storage, T value)
        {
            return storage.Equals(value);
        }
    }
}
