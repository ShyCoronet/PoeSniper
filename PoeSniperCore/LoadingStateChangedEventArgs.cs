using System;

namespace PoeSniperCore
{
    public class LoadingStateChangedEventArgs : EventArgs
    {
        public bool IsLoading { get; }

        public LoadingStateChangedEventArgs(bool isLoading)
        {
            IsLoading = isLoading;
        }
    }
}
