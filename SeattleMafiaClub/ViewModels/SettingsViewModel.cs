using System;
using System.Windows.Input;
using SeattleMafiaClub.Services;
using SeattleMafiaClub.Views;
using Xamarin.Forms;

namespace SeattleMafiaClub.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public SettingsViewModel(INavigation navigation)
        {
            Title = "Settings";

            OpenWebCommand = new Command(() => {
                AuthService.getInstance().forgetCredentials();
                navigation.PushModalAsync(new NavigationPage(new LoginPage()));
            });
        }

        public ICommand OpenWebCommand { get; }
    }
}
