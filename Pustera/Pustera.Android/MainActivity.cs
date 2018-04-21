using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Android.Views;
using Firebase.JobDispatcher;
using Pustera.Helpers;
using System;
using System.Linq;
using Xamarin.Forms.Platform.Android.AppLinks;

namespace Pustera.Droid
{
    [Activity(Label = "Pustera",
        Icon = "@drawable/icon",
        Theme = "@style/MainTheme",
        MainLauncher = true,
        WindowSoftInputMode = SoftInput.AdjustResize,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    //[IntentFilter(new[] { Intent.ActionView },
    //    Categories = new[] {
    //        Intent.ActionView,
    //        Intent.CategoryDefault,
    //        Intent.CategoryBrowsable
    //    },
    //    DataScheme = "http",
    //    DataHost = "pustera.com")
    //]
    //[IntentFilter(new[] { Intent.ActionView },
    //    Categories = new[] {
    //        Intent.ActionView,
    //        Intent.CategoryDefault,
    //        Intent.CategoryBrowsable
    //    },
    //    DataScheme = "https",
    //    DataHost = "pustera.com")
    //]
    //[IntentFilter(new[] { Intent.ActionView },
    //    Categories = new[] {
    //        Intent.ActionView,
    //        Intent.CategoryDefault,
    //        Intent.CategoryBrowsable
    //    },
    //    DataScheme = "http",
    //    DataHost = "www.pustera.com")
    //]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private const string NOTIF_URL = "NOTIFICATION_URL";

        protected override void OnCreate(Bundle bundle)
        {
            try
            {
                //TabLayoutResource = Resource.Layout.Tabbar;
                //ToolbarResource = Resource.Layout.Toolbar;

                base.OnCreate(bundle);

                global::Xamarin.Forms.Forms.Init(this, bundle);

                AndroidAppLinks.Init(this);

                if (Intent.HasExtra(NOTIF_URL))
                {
                    LoadApplication(new App(Intent.GetStringExtra(NOTIF_URL)));
                }
                else
                {
                    LoadApplication(new App());
                    ScheduleNotificationCheck();
                }
            }
            catch (Exception bug)
            {

            }
        }

        private void ScheduleNotificationCheck()
        {
            try
            {
                FirebaseJobDispatcher dispatcher = this.CreatJobDispatcher();
                Job job = dispatcher.NewJobBuilder()
                    .SetService<PusteraScheduler>("notifJob")
                    .SetRecurring(true)
                    .SetLifetime(Lifetime.Forever)
                    .SetRetryStrategy(dispatcher.NewRetryStrategy(RetryStrategy.RetryPolicyLinear, 30, 360))
                    .SetReplaceCurrent(false)
                    .SetTrigger(Trigger.ExecutionWindow(Utils.HoursToSeconds(1), Utils.HoursToSeconds(1.5)))
                    .Build();

                dispatcher.Schedule(job);
            }
            catch (Exception bug)
            {
                Log.Debug("PUSTERA_LOG", bug.Message);
            }

        }

        protected async override void OnNewIntent(Intent intent)
        {
            if (intent.HasExtra(NOTIF_URL))
            {
                MainPage page = new MainPage(intent.GetStringExtra(NOTIF_URL));

                await Xamarin.Forms.Application.Current.MainPage.Navigation
                    .PushAsync(page);
            }
            base.OnNewIntent(intent);
        }

        public override void OnBackPressed()
        {
            Xamarin.Forms.Page page = Xamarin.Forms.Application
                                        .Current.MainPage
                                        .Navigation.NavigationStack
                                        .LastOrDefault();

            if (page is MainPage mainPage)
            {
                mainPage.OnBackPressed();
            }
            else
            {
                base.OnBackPressed();
            }
        }
    }
}

