using System;

namespace PoeSniperCore.EventsArgs
{
    public class ConnectionStateChangedEventArgs : EventArgs
    {
        public bool IsConnected { get; }

        public ConnectionStateChangedEventArgs(bool state)
        {
            IsConnected = state;
        }
    }
}
