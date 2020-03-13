using IrrigationController.Model;
using IrrigationController.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;

namespace IrrigationController.Network
{

    public class LoginService : ILoginService
    {
        private static readonly string GOOGLE_CLIENT_ID = "128088518939-g0ht0124tg75nj1j661hqb21nccg4l70.apps.googleusercontent.com";
        private static readonly string GOOGLE_CLIENT_SECRET = "";
        public static readonly string GOOGLE_REDIRECT_URL = "com.googleusercontent.apps.128088518939-g0ht0124tg75nj1j661hqb21nccg4l70:/oauth2redirect";


        private HttpAPI _httpAPI;
        private OAuth2Authenticator _authenticator;
        private LoginSucceeded _loginSucceeded;
        private LoginFailed _loginFailed;
        private LoginCanceled _loginCanceled;

        public LoginService(HttpAPI httpAPI)
        {
            this._httpAPI = httpAPI;
        }

        public async void LoginWithGoogle(LoginSucceeded loginSucceeded, LoginFailed loginFailed, LoginCanceled loginCanceled)
        {
            _loginSucceeded = loginSucceeded;
            _loginFailed = loginFailed;
            _loginCanceled = loginCanceled;

            _authenticator = new OAuth2Authenticator(
               clientId: GOOGLE_CLIENT_ID,
               clientSecret: GOOGLE_CLIENT_SECRET,
               scope: "https://www.googleapis.com/auth/userinfo.email https://www.googleapis.com/auth/userinfo.profile",
               authorizeUrl: new Uri("https://accounts.google.com/o/oauth2/auth"),
               redirectUrl: new Uri(GOOGLE_REDIRECT_URL),
               accessTokenUrl: new Uri("https://www.googleapis.com/oauth2/v4/token"),
               isUsingNativeUI: true
            );

            _authenticator.Completed += AuthenticatorCompleted;
            _authenticator.Error += AuthenticatorError;

            AuthenticationState.Authenticator = _authenticator;
            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(_authenticator);
        }

        private async void AuthenticatorCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            _authenticator.Completed -= AuthenticatorCompleted;
            try
            {
                if (e.IsAuthenticated)
                {
                    var acc = e.Account;
                    var usr = await GetGoogleUser(acc.Properties["id_token"], e.Account);
                    var token = await SignInToAPI(usr, AuthProvider.Google);

                    _httpAPI.SetLoginToken(token);
                    _loginSucceeded?.Invoke(usr);
                }
                else
                {
                    _loginCanceled?.Invoke();
                }
            }
            catch(Exception err)
            {
                _loginFailed?.Invoke(err.Message);
            }
        }

        private void AuthenticatorError(object sender, AuthenticatorErrorEventArgs e)
        {
            _authenticator.Error -= AuthenticatorError;
            _loginFailed?.Invoke(e.Message);
        }

        private async Task<User> GetGoogleUser(string idToken, Account account)
        {
            //Get user's profile info from Google
            var request = new OAuth2Request("GET", new Uri("https://www.googleapis.com/oauth2/v2/userinfo"), null, account);
            var response = await request.GetResponseAsync();
            User user = null;

            if (response != null)
            {
                string userJson = await response.GetResponseTextAsync();
                user = JsonConvert.DeserializeObject<User>(userJson);
                user.TokenId = idToken;
                return user;
            }
            throw new Exception("Could not get user");
        }

        private async Task<string> SignInToAPI(User user, AuthProvider authProvider)
        {
            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var res = await _httpAPI.HttpClient.PostAsync(_httpAPI.GoogleLoginUrl(), content);

            var jwt = await res.Content.ReadAsStringAsync();
            Debug.WriteLine($"Token: Bearer {jwt}");
            return jwt;
        }

        public Task Logout()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> LoginCheck()
        {
            var res = await _httpAPI.HttpClient.GetAsync(_httpAPI.LoginCheckUrl());
            if (res.IsSuccessStatusCode) return true;
            else return false;
        }
    }

    public class AuthenticationState
    {
        public static OAuth2Authenticator Authenticator;
    }
}
