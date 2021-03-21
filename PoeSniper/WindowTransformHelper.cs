using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace PoeSniperUI
{
    enum ApiCodes
    {
        SC_RESTORE = 0xF120,
        SC_MINIMIZE = 0xF020,
        WM_SYSCOMMAND = 0x0112
    }

    public class WindowTransformHelper
    {
        private Window _window;
        private IntPtr _hWnd;

        public WindowTransformHelper(Window window)
        {
            _window = window;
        }

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            _hWnd = new WindowInteropHelper(_window).Handle;
            HwndSource.FromHwnd(_hWnd).AddHook(WindowProc);
        }

        public void Button_Click(object sender, RoutedEvent e)
        {
            SendMessage(_hWnd, (int)ApiCodes.WM_SYSCOMMAND, new IntPtr((int)ApiCodes.SC_MINIMIZE), IntPtr.Zero);
        }

        private IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == (int)ApiCodes.WM_SYSCOMMAND)
            {
                if (wParam.ToInt32() == (int)ApiCodes.SC_MINIMIZE)
                {
                    _window.WindowStyle = WindowStyle.SingleBorderWindow;
                    _window.WindowState = WindowState.Minimized;
                    handled = true;
                }
                else if (wParam.ToInt32() == (int)ApiCodes.SC_RESTORE)
                {
                    _window.WindowState = WindowState.Normal;
                    _window.WindowStyle = WindowStyle.None;
                    handled = true;
                }
            }

            return IntPtr.Zero;
        }
    }
}
