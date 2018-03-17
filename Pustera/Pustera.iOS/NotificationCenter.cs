using System;
using UserNotifications;

namespace Pustera.iOS
{
    class NotificationCenter : UNUserNotificationCenterDelegate
    {
        const string REQUEST_ID = "NOTIF_URL";

        public override void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
        {
            var content = response.Notification.Request.Content.MutableCopy() as UNMutableNotificationContent;
            if (response.IsDefaultAction && content != null)
            {
                var url = content.UserInfo[REQUEST_ID];

                var page = new MainPage()
                {
                    BaseUrl = url.ToString()
                };

                Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(page);
            }

            completionHandler();
        }
    }
}