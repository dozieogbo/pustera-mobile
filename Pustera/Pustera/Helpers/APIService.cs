using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Pustera.Helpers
{
    public class APIService
    {
        static HttpClient client = new HttpClient();

        private static string NotificationUrl
        {
            get
            {
                if (Device.RuntimePlatform == Device.iOS)
                    return "https://pustera.com/api.php?source=iOSApp";
                else if (Device.RuntimePlatform == Device.Android)
                    return "https://pustera.com/api.php?source=androidApp";
                else
                    return "https://pustera.com/api.php";
            }
            set { }
        }
        public static async Task<(Notification, string)> CheckNotification()
        {
            Notification notification = null;
            string message = null;

            try
            {
                HttpResponseMessage response = await client.GetAsync(NotificationUrl);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    List<Notification> notifs = JsonConvert.DeserializeObject<List<Notification>>(content);

                    Notification newNotif = notifs.FirstOrDefault();

                    DateTime lastDate = Settings.LastDate;

                    if (newNotif != null && newNotif.Date > lastDate)
                    {
                        notification = newNotif;
                        Settings.LastDate = newNotif.Date;
                    }

                    message = lastDate.ToString();
                }
                else
                {
                    message = response.Content.ToString();
                }
            }
            catch (Exception bug)
            {
                message = bug.Message;
                Debug.WriteLine(bug.Message);
            }

            return (notification, message);
        }
    }
}
