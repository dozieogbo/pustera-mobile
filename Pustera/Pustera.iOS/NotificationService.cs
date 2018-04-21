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
                    Body = message.Title
                };

                var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(5, false);

                content.UserInfo = new NSDictionary<NSString, NSString>(new NSString[] {
                    new NSString(REQUEST_ID), new NSString("TEXT_BODY") },
                    new NSString[] { new NSString(message.Url), new NSString(message.Title)
                    });

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