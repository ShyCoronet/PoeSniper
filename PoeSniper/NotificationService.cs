using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Notifications;

namespace PoeSniperUI
{
    public class NotificationService
    {
        public void ShowNotification(string name, string message)
        {
            new ToastContentBuilder().AddText(name).AddText(message).Show();
        }
    }
}
