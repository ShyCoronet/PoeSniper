using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace PoeSniperUI
{
    public class NotificationService
    {
        public void ShowNotification(string message)
        {
            string body = $@"<toast>
                      <visual>
                        <binding template='ToastGeneric'>
                          <text>TradeOffer</text>
                          <text>{message}</text>
                        </binding>
                      </visual>
                    </toast>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(body);

            ToastNotification toast = new ToastNotification(doc);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
    }
}
