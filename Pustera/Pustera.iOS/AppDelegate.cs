
using Foundation;
using System;
using UIKit;
using UserNotifications;

namespace Pustera.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        private const int FETCH_INTERVAL = 900; //2 * 60 * 60

        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary launchOptions)
        {
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            UNUserNotificationCenter.Current
                .RequestAuthorization(UNAuthorizationOptions.Alert,
                (approved, err) =>
                {
                    new NotificationService().Notify(new Notification
                    {
                        Date = DateTime.Now,
                        Url = "http://pustera.com",
                        Text = "Mmmmmmmmmm"
                    });
                });

            if (launchOptions != null)
            {
                // check for a local notification
                if (launchOptions.ContainsKey(UIApplication.LaunchOptionsLocalNotificationKey))
                {
                    if (launchOptions[UIApplication.LaunchOptionsLocalNotificationKey] is UILocalNotification localNotification)
                    {
                        UIAlertController okayAlertController = UIAlertController.Create(localNotification.AlertAction, localNotification.AlertBody, UIAlertControllerStyle.Alert);
                        okayAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));

                        Window.RootViewController.PresentViewController(okayAlertController, true, null);

                        // reset our badge
                        UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
                    }
                }
            }

            UIApplication.SharedApplication.SetMinimumBackgroundFetchInterval(FETCH_INTERVAL);

            return base.FinishedLaunching(app, launchOptions);
        }

        public override void PerformFetch(UIApplication application, Action<UIBackgroundFetchResult> completionHandler)
        {
            new NotificationService().Notify(new Notification
            {
                Date = DateTime.Now,
                Url = "http://pustera.com",
                Text = "Mmmmmmmmmm"
            });

            base.PerformFetch(application, completionHandler);
        }

        public override void ReceivedLocalNotification(UIApplication application, UILocalNotification notification)
        {
            try
            {
                const string REQUEST_ID = "NOTIF_URL";

                var url = notification.UserInfo[new NSString(REQUEST_ID)];

                //var page = new MainPage()
                //{
                //    BaseUrl = url.ToString()
                //};

                //Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(page);
            }
            catch (Exception bug)
            {

            }
        }

    }
}
