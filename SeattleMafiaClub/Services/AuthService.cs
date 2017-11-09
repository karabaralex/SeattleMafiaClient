using System;
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
            var authenticator = new Xamarin.Auth.OAuth2Authenticator(
                    "778099304554-5l1bgb9aq3o34dg40ak3pgll1547q19c.apps.googleusercontent.com",
                null,
                    "profile",
                    new Uri("https://accounts.google.com/o/oauth2/v2/auth"),
                    new Uri("com.seattlemafiaclub.SeattleMafiaClub:/oauth2redirect"),
                    new Uri("https://www.googleapis.com/oauth2/v4/token"),
                null,
                true);
            authenticator.Completed += (object s, Xamarin.Auth.AuthenticatorCompletedEventArgs e) => {
                if (e.IsAuthenticated)
                {
                    // The user is authenticated
                    // Extract the OAuth token
                    var token = new GoogleOAuthToken
                    {
                        TokenType =
                            e.Account.Properties["token_type"],
                        AccessToken =
                            e.Account.Properties["access_token"]
                    };

                    // Do something
                }
                else
                {
                    // The user is not authenticated
                }
            };

            authenticator.Error += (object s, Xamarin.Auth.AuthenticatorErrorEventArgs e) =>
            {
                s.ToString();
                if (OnCompletedListener != null)
                    OnCompletedListener(false);
            };

            this.authenticator = authenticator;
        }
    }
}
