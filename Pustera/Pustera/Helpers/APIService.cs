using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Pustera.Helpers
{
    public class APIService
    {
        static HttpClient client = new HttpClient();

        public string NotificationUrl
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
        public async Task<Notification> CheckNotification()
        {
            Notification notification = null;

            try
            {
                HttpResponseMessage response = await client.GetAsync(NotificationUrl);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Notification newNotif = JsonConvert.DeserializeObject<Notification>(content);

                    DateTime lastDate = Settings.LastDate;

                    if (newNotif.Date > lastDate)
                    {
                        notification = newNotif;
                        Settings.LastDate = newNotif.Date;
                    }
                }
            }
            catch (Exception bug)
            {
                Debug.WriteLine(bug.Message);
            }

            return notification;
        }
    }
}
