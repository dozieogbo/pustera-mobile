using Foundation;
using System;
using System.Diagnostics;
using UserNotifications;

namespace Pustera.iOS
{
    public class NotificationService : INotificationService
    {
        const string REQUEST_ID = "NOTIF_URL";
        public void Notify(Notification message)
        {
            try
            {
                var content = new UNMutableNotificationContent
                {
                    Title = "Alert",
                    Body = message.Text,
                    Badge = 3
                };

                var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(5, false);

                content.UserInfo = new NSDictionary<NSString, NSString>(new NSString(REQUEST_ID), new NSString(message.Url));

                var request = UNNotificationRequest.FromIdentifier(REQUEST_ID, content, trigger);


                UNUserNotificationCenter.Current.AddNotificationRequest(request, (err) =>
                {
                    if (err != null)
                    {
                        Debug.WriteLine(err.DebugDescription);
                    }
                    else
                    {

                    }
                });
            }
            catch (Exception bug)
            {
            }
        }
    }
}