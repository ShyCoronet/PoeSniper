using System;

namespace PoeSniperCore.EventsArgs
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
