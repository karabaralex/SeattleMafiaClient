using System;
using Xamarin.Forms;

namespace SeattleMafiaClub.Services
{
    public class AuthService
    {
        static AuthService instance = new AuthService();

        public Xamarin.Auth.OAuth2Authenticator authenticator;

        public static AuthService getInstance()
        {
            return instance;
        }

        public delegate void OnCompleted(bool result);

        public OnCompleted OnCompletedListener
        {
            get;set;
        }

        public AuthService()
        {
            //var authenticator = new Xamarin.Auth.OAuth2Authenticator(
            //"778099304554-5l1bgb9aq3o34dg40ak3pgll1547q19c.apps.googleusercontent.com",
            //null,
            //"profile",
            //new Uri("https://accounts.google.com/o/oauth2/v2/auth"),
            //new Uri("com.seattlemafiaclub.SeattleMafiaClub:/oauth2redirect"),
            //new Uri("https://www.googleapis.com/oauth2/v4/token"),
            //null,
            //true);
            //authenticator.Completed += (object s, Xamarin.Auth.AuthenticatorCompletedEventArgs e) => {
            //    if (e.IsAuthenticated)
            //    {
            //        // The user is authenticated
            //        // Extract the OAuth token
            //        var token = new GoogleOAuthToken
            //        {
            //            TokenType =
            //                e.Account.Properties["token_type"],
            //            AccessToken =
            //                e.Account.Properties["access_token"]
            //        };

            //        // Do something
            //    }
            //    else
            //    {
            //        // The user is not authenticated
            //    }
            //};

            //Xamarin.Auth.Account account = getFacebookToken();
            //if (account != null)
            //{
            //    queryUserInfo(account);
            //    return;
            //}

            var authenticator = new Xamarin.Auth.OAuth2Authenticator(
                clientId:"1965686263705363",
                scope:"email",
                authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"),
                redirectUrl: new Uri("http://www.facebook.com/connect/login_success.html"));
            
            authenticator.Completed += (object s, Xamarin.Auth.AuthenticatorCompletedEventArgs e) => {
                if (e.IsAuthenticated)
                {
                    // Do something
                    queryUserInfo(e.Account, (bool obj) => OnCompletedListener(obj));
                }
                else
                {
                    // The user is not authenticated
                    OnCompletedListener(false);
                }
            };

            authenticator.Error += (object s, Xamarin.Auth.AuthenticatorErrorEventArgs e) =>
            {
                if (OnCompletedListener != null)
                    OnCompletedListener(false);
            };

            this.authenticator = authenticator;
        }

        public void queryUserInfo(Action<bool> callback)
        {
            queryUserInfo(getFacebookToken(), callback);
        }

        private void queryUserInfo(Xamarin.Auth.Account account, Action<bool> callback)
        {
            var request = new Xamarin.Auth.OAuth2Request("GET", new Uri("https://graph.facebook.com/me"), null, account);

            request.GetResponseAsync().ContinueWith(t => {
                Device.BeginInvokeOnMainThread(() => {
                    if (t.IsFaulted)
                    {
                        callback(false);
                    }
                    else
                    {
                        setFacebookToken(account);
                        callback(true);
                    }
                });
            });
        }

        private Xamarin.Auth.Account getFacebookToken()
        {
            Xamarin.Auth.Account a = new Xamarin.Auth.Account();
            foreach (String key in Application.Current.Properties.Keys)
            {
                a.Properties.Add(key, Application.Current.Properties[key] as string);
            }
            return a;
        }

        private void setFacebookToken(Xamarin.Auth.Account token)
        {
            if (token == null)
            {
                Application.Current.Properties.Remove("facebook_access_token");
                return;
            }

            foreach (String key in token.Properties.Keys)
            {
                Application.Current.Properties[key] = token.Properties.GetValueOrDefault(key);
            }

            Application.Current.SavePropertiesAsync();
        }
    }
}
