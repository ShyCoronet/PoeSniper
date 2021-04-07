using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyboardHook;

namespace PoeSniperUI
{
    public class PasteCommandHandler
    {
        private GlobalKeyboardHook keyHook;
        private Tuple<Key, Key> hotKey = new(Key.LCONTROL, Key.KEY_V);
        private Key lastKey;

        public event EventHandler Pasted;

        public PasteCommandHandler()
        {
            keyHook = new GlobalKeyboardHook();
            keyHook.KeyDown += OnPaste;
        }

        public void Start()
        {
            keyHook.Install();
        }

        private void OnPaste(Key key)
        {
            if (key == hotKey.Item1)
            {
                lastKey = key;
            }

            if (lastKey == hotKey.Item1 && key == hotKey.Item2)
            {
                lastKey = default;
                Pasted?.Invoke(null, null);
            }
        }


        ~PasteCommandHandler()
        {
            keyHook.Uninstall();
        }
    }
}
