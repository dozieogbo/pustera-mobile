using Android.App;
using Android.App.Job;
using System;
using System.Threading.Tasks;

namespace Pustera.Droid
{
    [Service(Exported = true, Permission = PermissionBind)]
    class PusteraScheduler : JobService
    {
        public override bool OnStartJob(JobParameters @params)
        {
            CheckNotification(@params);
            return true;
        }

        private async Task CheckNotification(JobParameters @params)
        {
            new NotificationService(this).Notify(new Notification
            {
                Text = "A ahhams smammamam amamamjdjdm amammdjdjjd",
                Url = "http://www.pustera.com",
                Date = DateTime.Now,
            });
            JobFinished(@params, true);
        }

        public override bool OnStopJob(JobParameters @params)
        {
            return true;
        }
    }
}