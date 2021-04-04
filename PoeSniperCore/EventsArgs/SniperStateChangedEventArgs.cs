using System;

namespace PoeSniperCore.EventsArgs
{
    public class SniperStateChangedEventArgs : EventArgs
    {
        public bool IsActive { get; }

        public SniperStateChangedEventArgs(bool state)
        {
            IsActive = state;
        }
    }
}
