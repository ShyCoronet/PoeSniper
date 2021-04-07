using System.ComponentModel;
using System.Runtime.CompilerServices;

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
