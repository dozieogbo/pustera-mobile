
using System;
using Xamarin.Forms;

namespace Pustera
{
    public partial class App : Application
    {
        public App(string baseUrl)
        {
            InitializeComponent();
            InitializeSettings();

            MainPage page = new MainPage(baseUrl);

            MainPage = new NavigationPage(page);
        }

        public App()
        {
            InitializeComponent();
            InitializeSettings();

            MainPage = new NavigationPage(new MainPage());
        }

        public void InitializeSettings()
        {
            Xamarin.Forms.PlatformConfiguration.AndroidSpecific.Application
                .SetWindowSoftInputModeAdjust(this, Xamarin.Forms.PlatformConfiguration.AndroidSpecific.WindowSoftInputModeAdjust.Resize);
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
        protected override async void OnAppLinkRequestReceived(Uri uri)
        {
            MainPage page = new MainPage
            {
                BaseUrl = uri.ToString()
            };

            await MainPage.Navigation.PushAsync(page);

            base.OnAppLinkRequestReceived(uri);
        }
    }
}
