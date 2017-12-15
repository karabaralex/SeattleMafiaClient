using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace SeattleMafiaClub.Services
{
    public class AuthService
    {
        static AuthService instance = new AuthService();

        public Xamarin.Auth.OAuth2Authenticator authenticator;
        private readonly object authLock = new object();

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
                //redirectUrl: new Uri("http://com.seattlemafiaclub.SeattleMafiaClub/oauth2redirect"));
                redirectUrl: new Uri("http://www.facebook.com/connect/login_success.html"));
            System.Diagnostics.Debug.WriteLine("--- start auth");
            authenticator.Completed += (object s, Xamarin.Auth.AuthenticatorCompletedEventArgs e) => {
                System.Diagnostics.Debug.WriteLine("--- auth completed:" + e.IsAuthenticated);
                if (e.IsAuthenticated)
                {
                    setFacebookToken(e.Account);

                    // Do something
                    //queryUserInfo(e.Account, (bool obj) => OnCompletedListener(obj));
                    OnCompletedListener(true);
                }
                else
                {
                    // The user is not authenticated
                    OnCompletedListener(false);
                }
            };

            authenticator.Error += (object s, Xamarin.Auth.AuthenticatorErrorEventArgs e) =>
            {
                System.Diagnostics.Debug.WriteLine("--- auth error:" + e.Message);
                if (OnCompletedListener != null)
                    OnCompletedListener(false);
            };

            this.authenticator = authenticator;
        }

        public void queryUserInfo(Action<bool> callback)
        {
            var request = new Xamarin.Auth.OAuth2Request("GET", new Uri("https://graph.facebook.com/me"), null, getFacebookToken());

            request.GetResponseAsync().ContinueWith(t => {
                Device.BeginInvokeOnMainThread(() => {
                    System.Diagnostics.Debug.WriteLine("--- auth result:" + t.IsFaulted);
                    if (t.IsFaulted)
                    {
                        callback(false);
                    }
                    else
                    {
                        callback(true);
                    }
                });
            });
        }

        public void forgetCredentials()
        {
            setFacebookToken(null);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        internal string getToken()
        {
            Xamarin.Auth.Account account = getFacebookToken();
            if (account == null)
                return null;
            return account.Properties["access_token"];
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private Xamarin.Auth.Account getFacebookToken()
        {
            Xamarin.Auth.Account a = new Xamarin.Auth.Account();
            foreach (String key in Application.Current.Properties.Keys)
            {
                a.Properties.Add(key, Application.Current.Properties[key] as string);
            }
            return a;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void setFacebookToken(Xamarin.Auth.Account token)
        {
            if (token == null)
            {
                Application.Current.Properties.Remove("access_token");
                Application.Current.SavePropertiesAsync();
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
