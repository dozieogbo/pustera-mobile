
using Android.App;
using Android.App.Job;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using Xamarin.Forms.Platform.Android.AppLinks;

namespace Pustera.Droid
{
    [Activity(Label = "Pustera", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [IntentFilter(new[] { Intent.ActionView },
        Categories = new[] {
            Intent.ActionView,
            Intent.CategoryDefault,
            Intent.CategoryBrowsable
        },
        DataScheme = "http",
        DataHost = "pustera.com")
    ]
    [IntentFilter(new[] { Intent.ActionView },
        Categories = new[] {
            Intent.ActionView,
            Intent.CategoryDefault,
            Intent.CategoryBrowsable
        },
        DataScheme = "https",
        DataHost = "pustera.com")
    ]
    [IntentFilter(new[] { Intent.ActionView },
        Categories = new[] {
            Intent.ActionView,
            Intent.CategoryDefault,
            Intent.CategoryBrowsable
        },
        DataScheme = "http",
        DataHost = "www.pustera.com")
    ]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private const string NOTIF_URL = "NOTIFICATION_URL";

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

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

        private void ScheduleNotificationCheck()
        {
            ComponentName componentName = new ComponentName(this, Java.Lang.Class.FromType(typeof(PusteraScheduler)));
            JobInfo jobInfo = new JobInfo.Builder(1500, componentName)
                .SetPeriodic(2 * 60 * 60 * 1000) //2 hours
                .Build();


            JobScheduler scheduler = (JobScheduler)GetSystemService(JobSchedulerService);

            if (scheduler.GetPendingJob(1500) == null)
            {
                int result = scheduler.Schedule(jobInfo);

                if (result.Equals(JobScheduler.ResultSuccess))
                    Toast.MakeText(this, "Scheduled successfully", ToastLength.Short).Show();
            }
        }

        protected async override void OnNewIntent(Intent intent)
        {
            if (intent.HasExtra(NOTIF_URL))
            {
                MainPage page = new MainPage
                {
                    BaseUrl = intent.GetStringExtra(NOTIF_URL)
                };

                await Xamarin.Forms.Application.Current.MainPage.Navigation
                    .PushAsync(page);
            }
            base.OnNewIntent(intent);
        }
    }
}

