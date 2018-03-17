using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Provider;

namespace Pustera.Droid
{
    public class NotificationService : INotificationService
    {
        Context Context;
        const int PENDING_REQUEST = 1000;
        const int NOTIFICATION_ID = 10035;

        public NotificationService(Context context)
        {
            Context = context;
        }
        public void Notify(Notification message)
        {
            Intent intent = new Intent(Context, typeof(MainActivity));
            intent.PutExtra("NOTIFICATION_URL", message.Url);
            PendingIntent pendingIntent =
                PendingIntent.GetActivity(Context, PENDING_REQUEST, intent, PendingIntentFlags.OneShot);


#pragma warning disable CS0618 // Type or member is obsolete
            Android.App.Notification notification = new Android.App.Notification.Builder(Context)
                    .SetContentIntent(pendingIntent)
                    .SetAutoCancel(true)
                    .SetContentTitle("Alert")
                    .SetContentText(message.Text)
                    .SetSmallIcon(Resource.Drawable.icon)
                        .SetVibrate(new long[] { 1000, 1000 })
                        .SetSound(Settings.System.DefaultNotificationUri)
#pragma warning restore CS0618 // Type or member is obsolete
                    .SetLargeIcon(((BitmapDrawable)Context.GetDrawable(Resource.Drawable.icon)).Bitmap)
                    .Build();

            NotificationManager notificationManager =
                Context.GetSystemService(Context.NotificationService) as NotificationManager;

            notificationManager.Notify(NOTIFICATION_ID, notification);
        }
    }
}