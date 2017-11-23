using System;
using SeattleMafiaClub.Views;
using Xamarin.Forms;

namespace SeattleMafiaClub
{
    public partial class App : Application
    {
        public static string BackendUrl = "https://bereg-online.com/lux-mg/rest";

        public App()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.iOS)
                MainPage = new NavigationPage(new LoginPage());
            else
                MainPage = new NavigationPage(new LoginPage());
        }
    }
}
