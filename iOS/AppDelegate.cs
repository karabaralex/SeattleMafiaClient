using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using SeattleMafiaClub.Services;
using UIKit;

namespace SeattleMafiaClub.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Auth.Presenters.XamarinIOS.AuthenticationConfiguration.Init();
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());



            return base.FinishedLaunching(app, options);
        }

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            var uri_netfx = new Uri(url.AbsoluteString);
            AuthService.getInstance().authenticator.OnPageLoading(uri_netfx);
            return true;
        }
    }
}
