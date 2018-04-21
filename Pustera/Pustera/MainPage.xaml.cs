using Pustera.Helpers;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Pustera
{
    public partial class MainPage : ContentPage
    {
        public bool IsIOS => Device.RuntimePlatform == Device.iOS;
        public bool IsAndroid => Device.RuntimePlatform == Device.Android;


        public string BaseUrl
        {
            get
            {
                if (IsIOS)
                    return "https://pustera.com?source=iOSApp";
                else if (IsAndroid)
                    return "https://pustera.com?source=androidApp";
                else
                    return "https://pustera.com";
            }
            set
            {
            }
        }

        private string Host = "pustera.com";

        public MainPage() : this(null)
        {

        }

        public MainPage(string baseUrl)
        {
            try
            {
                InitializeComponent();

                if (IsAndroid)
                {
                    NavigationPage.SetHasNavigationBar(this, false);
                }

                if (MWebView != null)
                {

                    MWebView.Source = baseUrl ?? BaseUrl;
                    MWebView.Navigating += OnNavigating;
                    MWebView.Navigated += OnEndNavigating;

                }

            }
            catch (Exception bug)
            {
            }
        }

        public void OnNavigating(object sender, WebNavigatingEventArgs e)
        {
            Uri uri = new Uri(e.Url);

            if ("https".Contains(uri.Scheme) && uri.Host.Equals(Host))
            {
                MLoader.IsVisible = true;
                MLoader.IsRunning = true;
                MWebView.Opacity = 0.25;
            }
            else
            {
                try
                {
                    Device.OpenUri(uri);
                }
                catch (Exception bug)
                {
                    DisplayAlert("Error", bug.Message, "Ok");
                }
                e.Cancel = true;
            }
        }

        public void OnEndNavigating(object sender, WebNavigatedEventArgs e)
        {
            MLoader.IsVisible = false;
            MLoader.IsRunning = false;
            MWebView.Opacity = 1;
        }

        public bool OnBackPressed()
        {
            return OnBackButtonPressed();
        }

        protected override bool OnBackButtonPressed()
        {
            if (MWebView.CanGoBack)
            {
                MWebView.GoBack();
                return false;
            }
            else
            {
                ExitOnRequest();
                return true;
            }
        }

        private async Task ExitOnRequest()
        {
            bool accept = await DisplayAlert("Warning", "Are you sure you want to exit?", "Yes", "No");
            if (accept)
            {
                ICloser closer = DependencyService.Get<ICloser>();
                if (closer != null)
                {
                    closer.Close();
                }
            }
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
