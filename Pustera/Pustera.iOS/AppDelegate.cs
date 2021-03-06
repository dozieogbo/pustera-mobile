﻿
using Foundation;
using Pustera.Helpers;
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
                });

            //if (launchOptions != null)
            //{
            //    // check for a local notification
            //    if (launchOptions.ContainsKey(UIApplication.LaunchOptionsLocalNotificationKey))
            //    {
            //        if (launchOptions[UIApplication.LaunchOptionsLocalNotificationKey] is UILocalNotification localNotification)
            //        {
            //            UIAlertController okayAlertController = UIAlertController.Create(localNotification.AlertAction, localNotification.AlertBody, UIAlertControllerStyle.Alert);
            //            okayAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));

            //            Window.RootViewController.PresentViewController(okayAlertController, true, null);

            //            // reset our badge
            //            UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
            //        }
            //    }
            //}
            UNUserNotificationCenter.Current.Delegate = new NotificationCenter();

            UIApplication.SharedApplication.SetMinimumBackgroundFetchInterval(UIApplication.BackgroundFetchIntervalMinimum);

            return base.FinishedLaunching(app, launchOptions);
        }

        public async override void PerformFetch(UIApplication application, Action<UIBackgroundFetchResult> completionHandler)
        {
            (Notification notification, string message) = await APIService.CheckNotification();

            if (notification != null)
            {
                new NotificationService().Notify(notification);
                completionHandler(UIBackgroundFetchResult.NewData);
            }
            else
            {
                completionHandler(UIBackgroundFetchResult.NoData);
            }
        }

        public override void ReceivedLocalNotification(UIApplication application, UILocalNotification notification)
        {
            try
            {
                const string REQUEST_ID = "NOTIF_URL";

                string url = notification.UserInfo[new NSString(REQUEST_ID)].ToString();
                string body = notification.UserInfo[new NSString("TEXT_BODY")].ToString();

                var alertController = UIAlertController.Create("Alert", body, UIAlertControllerStyle.Alert);

                //Add Actions
                alertController.AddAction(UIAlertAction.Create("View", UIAlertActionStyle.Default,
                    async alert =>
                {
                    MainPage page = new MainPage(url);
                    await Xamarin.Forms.Application.Current.MainPage.Navigation
                    .PushAsync(page);
                }));

                alertController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, alert =>
                {

                }));

                var window = UIApplication.SharedApplication.KeyWindow;
                window.RootViewController.PresentViewController(alertController, true, null);
            }
            catch (Exception bug)
            {

            }
        }

    }
}
