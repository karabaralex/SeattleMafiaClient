using System;
using SeattleMafiaClub.Services;
using Xamarin.Forms;

namespace SeattleMafiaClub.Views
{
    public class LoginPage : ContentPage
    {
        public LoginPage()
        {
            Content = new StackLayout
            {
                Children = {
                    new ProgressBar()
                }
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            query();
        }

        private void query()
        {
            AuthService.getInstance().queryUserInfo(async (result) =>
            {
                if (result)
                {
                        await Navigation.PushAsync(new ItemsPage());
                        Navigation.RemovePage(this);
                }
                else
                {
                    //todo multiple times
                    AuthService.getInstance().OnCompletedListener = async (result2) =>
                    {
                        if (result2)
                        {
                            await Navigation.PushAsync(new ItemsPage());
                            Navigation.RemovePage(this);
                        }
                        else
                            await Navigation.PopToRootAsync();
                    };

                    var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
                    presenter.Login(AuthService.getInstance().authenticator);
                }
            });
        }
    }
}

