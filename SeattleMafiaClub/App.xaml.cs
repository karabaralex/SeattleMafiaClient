using System;
using SeattleMafiaClub.Views;
using Xamarin.Forms;

namespace SeattleMafiaClub
{
    public partial class App : Application
    {
        public static bool UseMockDataStore = true;
        public static string BackendUrl = "https://localhost:5000";

        public App()
        {
            InitializeComponent();

            if (UseMockDataStore)
                DependencyService.Register<MockDataStore>();
            else
                DependencyService.Register<CloudDataStore>();

            if (Device.RuntimePlatform == Device.iOS)
                MainPage = new NavigationPage(new LoginPage());
            else
                MainPage = new NavigationPage(new LoginPage());
        }
    }
}
