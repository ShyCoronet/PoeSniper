using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoeSniperCore
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
