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
