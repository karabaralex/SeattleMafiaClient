using System;
using SeattleMafiaClub.Services;
using Xamarin.Forms;

namespace SeattleMafiaClub
{
    public class MainPage : TabbedPage
    {
        public MainPage()
        {
            //Button button = new Button
            //{
            //    Text = "Hello, Forms!",
            //    VerticalOptions = LayoutOptions.CenterAndExpand,
            //    HorizontalOptions = LayoutOptions.CenterAndExpand,
            //};

            //button.Clicked += (sender, e) => query();
            //Content = button;
            Page itemsPage, aboutPage = null;
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    itemsPage = new NavigationPage(new ItemsPage());

                    aboutPage = new NavigationPage(new AboutPage())
                    {
                        Title = "About"
                    };
                    itemsPage.Icon = "tab_feed.png";
                    aboutPage.Icon = "tab_about.png";
                    break;
                default:
                    itemsPage = new ItemsPage();

                    aboutPage = new AboutPage()
                    {
                        Title = "About"
                    };
                    break;
            }

            Children.Add(itemsPage);
            Children.Add(aboutPage);

            Title = Children[0].Title;
        }
    }
}
