using Android.App;
using Android.Util;
using Firebase.JobDispatcher;
using Pustera.Helpers;
using System;
using System.Threading.Tasks;

namespace Pustera.Droid
{
    [Service(Exported = true, Name = "com.pustera.app.scheduler")]
    class PusteraScheduler : JobService
    {
        public override bool OnStartJob(IJobParameters @params)
        {
            CheckNotification(@params);
            return true;
        }

        private async Task CheckNotification(IJobParameters @params)
        {
            try
            {
                Log.Debug("PUSTERA_TAG", "Starting notifiation check");

                (Notification notification, string message) = await APIService.CheckNotification();

                if (notification != null)
                {
                    var notifService = new NotificationService(this);
                    notifService.Notify(notification);
                }

                Log.Error("PUSTERA_TAG", "Finished message.." + message + " :");

                Log.Debug("PUSTERA_TAG", "Finished notifiation check.." + notification?.Date ?? "");


                JobFinished(@params, true);

                Log.Debug("PUSTERA_TAG", "Job finished");
            }
            catch (Exception bug)
            {
                Log.Debug("PUSTERA_TAG", bug.Message);
            }
        }

        public override bool OnStopJob(IJobParameters @params)
        {
            return false;
        }
    }
}