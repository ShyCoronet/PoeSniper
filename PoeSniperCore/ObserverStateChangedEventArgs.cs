using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoeSniperCore
{
    public class ObserverStateChangedEventArgs : EventArgs
    {
        public bool IsActive { get; }

        public ObserverStateChangedEventArgs(bool state)
        {
            IsActive = state;
        }
    }
}
