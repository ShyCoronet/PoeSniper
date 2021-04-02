using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoeSniperCore
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
