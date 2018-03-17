
using System;
using Xamarin.Forms;

namespace Pustera
{
    public partial class App : Application
    {
        public App(string baseUrl = null)
        {
            InitializeComponent();

            MainPage page = new MainPage();

            if (!string.IsNullOrEmpty(baseUrl))
                page.BaseUrl = baseUrl;

            MainPage = new NavigationPage(page);
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
