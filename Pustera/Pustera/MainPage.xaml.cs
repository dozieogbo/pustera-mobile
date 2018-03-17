using System;
using Xamarin.Forms;

namespace Pustera
{
    public partial class MainPage : ContentPage
    {

        public string BaseUrl
        {
            get
            {
                if (Device.RuntimePlatform == Device.iOS)
                    return "https://pustera.com?source=iOS";
                else if (Device.RuntimePlatform == Device.Android)
                    return "https://pustera.com?source=androidApp";
                else
                    return "https://pustera.com";
            }
            set
            {
            }
        }

        private string Host = "pustera.com";

        public MainPage()
        {
            InitializeComponent();
            MWebView.Source = BaseUrl;
        }

        void OnNavigating(object sender, WebNavigatingEventArgs e)
        {
            if (e.Url.Contains(Host))
            {
                Loader.IsVisible = true;
            }
            else
            {
                try
                {
                    var uri = new Uri(e.Url);
                    Device.OpenUri(uri);
                }
                catch (Exception bug)
                {
                    DisplayAlert("Error", bug.Message, "Ok");
                }
            }
        }

        void OnEndNavigating(object sender, WebNavigatedEventArgs e)
        {
            Loader.IsVisible = false;
        }

        protected override bool OnBackButtonPressed()
        {
            if (MWebView.CanGoBack)
            {
                MWebView.GoBack();
                return false;
            }
            else
                return base.OnBackButtonPressed();
        }

        protected override void OnAppearing()
        {
            var url = $"http://pustera.com";

            var entry = new AppLinkEntry
            {
                Title = "Pustera",
                AppLinkUri = new Uri(url, UriKind.RelativeOrAbsolute),
                IsLinkActive = true
            };

            entry.KeyValues.Add("appName", "Pustera");
            entry.KeyValues.Add("companyName", "Pustera");

            //Application.Current.AppLinks.RegisterLink(entry);

            base.OnAppearing();
        }
    }
}
